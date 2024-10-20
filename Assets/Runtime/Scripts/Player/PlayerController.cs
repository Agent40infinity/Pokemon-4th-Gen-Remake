﻿using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Runtime.Overworld;

namespace Runtime.Player
{
    public class PlayerController : MonoBehaviour, IPlayer
    {
        public Dictionary<KeyCode, Vector3> keyDict = new Dictionary<KeyCode, Vector3>()
        {
            { KeyCode.W, new Vector3(0, 0, 1) },
            { KeyCode.A, new Vector3(-1, 0, 0) },
            { KeyCode.S, new Vector3(0, 0, -1) },
            { KeyCode.D, new Vector3(1, 0, 0) },
            { KeyCode.None, new Vector3(0, 0, 0 ) }
        };

        public List<KeyCode> lastKeys = new List<KeyCode>();
        public KeyCode LastKey;

        public float playerSpeed = 2f;
        private Vector3 toMove = new Vector3();

        private bool running = false;

        public Vector3 CurrentKey()
            => lastKeys.Count() > 0 ? keyDict[lastKeys.Last()] : Vector3.zero;

        public Vector3 FutureDist()
            => toMove - transform.position;

        public Vector3 PastPos()
            => toMove - keyDict[LastKey];

        public bool IsMoving()
            => !OverworldPhysics.WithinTile(toMove, transform) || lastKeys.Count > 0;

        public bool Running()
            => running;

        public float Speed()
            => playerSpeed;

        public void Update()
        {
            GetInput();
            Physics();
        }

        public void GetInput()
        {
            foreach (var key in keyDict.Keys)
            {
                if (Input.GetKeyDown(key))
                {
                    lastKeys.Add(key);
                    LastKey = key;
                }

                if (Input.GetKeyUp(key))
                {
                    lastKeys.Remove(key);
                }
            }

            running = Input.GetKey(KeyCode.LeftShift);
        }

        public void Physics()
        {
            if (OverworldPhysics.WithinTile(toMove, transform))
            {
                if (lastKeys.Count > 0)
                {
                    toMove = transform.position;
                    toMove += keyDict[lastKeys.Last()];
                    OverworldPhysics.RoundTowards(toMove);
                }
            }
           
            transform.position = OverworldPhysics.Update(transform, ref toMove, playerSpeed, running);
        }
    }
}