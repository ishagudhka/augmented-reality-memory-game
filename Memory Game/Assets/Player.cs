using UnityEngine;
using System.Collections;
using Vuforia;
using System;
using System.Collections.Generic;

public class Player : MonoBehaviour {
    
    public static int ViradaAtual = -1;
    public static int UltimaVirada = -1;

    public List<int> CartasViradas = new List<int>();
    
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
                int valor = markerBehaviour.Marker.MarkerID % 2 == 0 ? markerBehaviour.Marker.MarkerID : markerBehaviour.Marker.MarkerID - 1;

                for (int i = 0; i < CartasViradas.Count; i++)
                {
                    Debug.Log("IF: " + CartasViradas[i] + " - " + valor);

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

                        Debug.Log("Virou o primeiro: " + ViradaAtual);
                    }
                    else if (ViradaAtual != markerBehaviour.Marker.MarkerID)
                    {
                        bool par = false;
                        if (ViradaAtual % 2 == 0)
                            par = true;

                        if ((par && markerBehaviour.Marker.MarkerID == ViradaAtual + 1) ||
                            (!par && markerBehaviour.Marker.MarkerID == ViradaAtual - 1))
                        {
                            CartasViradas.Add(par ? ViradaAtual : ViradaAtual - 1);
                            Debug.Log("Par Encontrado [" + ViradaAtual + ", " + markerBehaviour.Marker.MarkerID + "]");

                            ViradaAtual = -1;
                        }
                        else
                        {
                            Debug.Log("Par errado, tente novamente.");
                            ViradaAtual = -1;
                        }
                    }

                    UltimaVirada = markerBehaviour.Marker.MarkerID;
                }
            }
        }
    }
}
