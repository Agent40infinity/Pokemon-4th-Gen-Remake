﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerAnimation : MonoBehaviour
    {
        private Animator anim;
        IPlayer player;

        /* Direction determined by float to make it easier to control since we can't do a state machine for animations.
         * 1 - Up
         * 2 - Left
         * 3 - Down
         * 4 - Right
         */
        private float facing = 3;

        public void Awake()
        {
            anim = GetComponent<Animator>();
            player = GetComponent<IPlayer>();
        }

        public void Update()
        {
            anim.SetFloat("velocityX", player.FuturePos().x);
            anim.SetFloat("velocityY", player.FuturePos().z);
            anim.SetBool("running", player.Running());

            DetermineLastFacing();

            anim.SetFloat("facing", facing);
        }

        public void DetermineLastFacing()
        {
            var dir = player.FuturePos();

            switch (true)
            {
                case var _ when dir.x <= -0.9:
                    facing = 2;
                    break;
                case var _ when dir.x >= 0.9:
                    facing = 4;
                    break;
                case var _ when dir.z >= 0.9:
                    facing = 1;
                    break;
                case var _ when dir.z <= -0.9:
                    facing = 3;
                    break;
            }
        }
    }
}
