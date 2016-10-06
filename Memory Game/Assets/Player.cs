using UnityEngine;
using System.Collections;
using Vuforia;
using System;
using System.Collections.Generic;

public class Player : MonoBehaviour {
    
    public static int ViradaAtual = -1;
    public static int UltimaVirada = -1;
    public static bool TodasViradas = false;

    public List<int> CartasViradas = new List<int>();

    private bool ParEncontrado = false;
    private bool ParErrado = false;
    
    // Use this for initialization
    void Start ()
    {
        
    }
	
	// Update is called once per frame
	void Update ()
    {
        StateManager stateManager = TrackerManager.Instance.GetStateManager();

        IEnumerable<TrackableBehaviour> trackables = stateManager.GetActiveTrackableBehaviours();

        foreach (TrackableBehaviour track in trackables)
        {
            if (track.GetType() == typeof(MarkerBehaviour))
            {
                MarkerBehaviour markerBehaviour = (MarkerBehaviour)track;
                
                bool CartaJaVirada = false;
                int valor = markerBehaviour.Marker.MarkerID % 2 == 0 ? markerBehaviour.Marker.MarkerID : markerBehaviour.Marker.MarkerID + 1;

                for (int i = 0; i < CartasViradas.Count; i++)
                {
                    if (CartasViradas[i] == valor)
                    {
                        CartaJaVirada = true;
                        break;
                    }
                }

                if (!CartaJaVirada && markerBehaviour.Marker.MarkerID != UltimaVirada)
                {
                    if (ViradaAtual == -1)
                    {
                        ViradaAtual = markerBehaviour.Marker.MarkerID;

                        ParEncontrado = false;
                        ParErrado = false;

                        Debug.Log("Virou o primeiro: " + ViradaAtual);

                        UltimaVirada = markerBehaviour.Marker.MarkerID;
                    }
                    else if (ViradaAtual != markerBehaviour.Marker.MarkerID)
                    {
                        bool par = false;
                        if (ViradaAtual % 2 == 0)
                            par = true;

                        Debug.Log("Virou o segundo: " + markerBehaviour.Marker.MarkerID);

                        if ((par && markerBehaviour.Marker.MarkerID == ViradaAtual - 1) ||
                            (!par && markerBehaviour.Marker.MarkerID == ViradaAtual + 1))
                        {
                            CartasViradas.Add(par ? ViradaAtual : ViradaAtual + 1);
                            Debug.Log("Par Encontrado [" + ViradaAtual + ", " + markerBehaviour.Marker.MarkerID + "]");

                            ParEncontrado = true;

                            ViradaAtual = -1;

                            UltimaVirada = markerBehaviour.Marker.MarkerID;

                            if (CartasViradas.Count >= 8)
                                TodasViradas = true;
                        }
                        else
                        {
                            Debug.Log("Par errado, tente novamente.");

                            ParErrado = true;

                            UltimaVirada = -1;
                            ViradaAtual = -1;
                        }
                    }
                }
            }
        }
    }

    void OnGUI()
    {
        if (ParEncontrado && !TodasViradas)
        {
            GUI.Label(new Rect(50, 50, 500, 50), "<color=green><size=30>" + CartasViradas.Count + "º Par Encontrado!</size></color>");
        }

        if (TodasViradas)
        {
            GUI.Label(new Rect(50, 50, 1000, 50), "<color=blue><size=30>Parabéns! Você encontrou todos os pares!</size></color>");
        }

        if (ParErrado)
        {
            GUI.Label(new Rect(50, 50, 500, 50), "<color=red><size=30>Par Inválido!</size></color>");
        }
    }
}
