//================================================================================================================================
//
//  Copyright (c) 2015-2025 VisionStar Information Technology (Shanghai) Co., Ltd. All Rights Reserved.
//  EasyAR is the registered trademark or trademark of VisionStar Information Technology (Shanghai) Co., Ltd in China
//  and other countries for the augmented reality technology developed by VisionStar Information Technology (Shanghai) Co., Ltd.
//
//================================================================================================================================

using UnityEngine;

namespace easyar
{
    class SessionWorkflowValidator
    {
        private ARSession session;
        private Optional<ARSession.SessionState> state;

        public bool CanInitialize => Application.isPlaying && !EasyARController.IsReady;
        public bool CanAssemble => Application.isPlaying && EasyARController.IsReady && state.OnSome && state.Value < ARSession.SessionState.Assembling;
        public bool CanStartAssembledSession => Application.isPlaying && EasyARController.IsReady && state == ARSession.SessionState.Assembled;
        public bool CanStopSession => Application.isPlaying && EasyARController.IsReady && state.OnSome && state.Value >= ARSession.SessionState.Ready;
        public bool CanDeinitialize => Application.isPlaying && EasyARController.IsReady;
        private bool CanStartSession => Application.isPlaying && EasyARController.IsReady && state.OnSome && state.Value < ARSession.SessionState.Ready;

        public SessionWorkflowValidator(ARSession session)
        {
            this.session = session;
            if (!session) { return; }
            state = session.State;
            session.StateChanged += (state) => { this.state = state; };
        }

        public bool IsValid(ARSession session) => this.session == session;

        public void Initialize()
        {
            if (!CanInitialize) { return; }
            EasyARController.Initialize();
        }

        public void Initialize(string license)
        {
            if (!CanInitialize) { return; }
            EasyARController.Initialize(license);
        }

        public void Assemble()
        {
            if (!CanAssemble) { return; }
            session.StartCoroutine(session.Assemble());
        }

        public void StartSession()
        {
            if (!CanStartSession) { return; }
            session.StartSession();
        }

        public void StopSession(bool keepLastFrame)
        {
            if (!CanStopSession) { return; }
            session.StopSession(keepLastFrame);
        }

        public void Deinitialize()
        {
            if (!CanDeinitialize) { return; }
            EasyARController.Deinitialize();
        }
    }
}
