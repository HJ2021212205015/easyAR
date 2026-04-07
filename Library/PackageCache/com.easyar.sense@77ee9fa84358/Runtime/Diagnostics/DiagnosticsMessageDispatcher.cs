
//================================================================================================================================
//
//  Copyright (c) 2015-2025 VisionStar Information Technology (Shanghai) Co., Ltd. All Rights Reserved.
//  EasyAR is the registered trademark or trademark of VisionStar Information Technology (Shanghai) Co., Ltd in China
//  and other countries for the augmented reality technology developed by VisionStar Information Technology (Shanghai) Co., Ltd.
//
//================================================================================================================================

using System;
using UnityEngine;

namespace easyar
{
    internal class DiagnosticsMessageDispatcher : MonoBehaviour
    {
        CautionBoard cautionBoard;
        CautionPanel cautionPanel;
        DumpBoard dumpBoard;
        DumpPanel dumpPanel;
        MessageBoard messageBoard;
        MessagePanel messagePanel;

        public void Setup(bool isHMD)
        {
            if (isHMD)
            {
                if (cautionPanel) { Destroy(cautionPanel); }
                if (dumpPanel) { Destroy(dumpPanel); }
                if (messagePanel) { Destroy(messagePanel); }
            }
            else
            {
                if (cautionBoard) { Destroy(cautionBoard); }
                if (dumpBoard) { Destroy(dumpBoard); }
                if (messageBoard) { Destroy(messageBoard); }
            }
        }

        public Action<string> GetCautionHandler(Optional<bool> isHMD, GameObject origin)
        {
            Setup(DiagnosticsMessageType.Caution, isHMD);
            if (cautionBoard && cautionPanel)
            {
                return (m) =>
                {
                    cautionBoard.Setup(origin);
                    cautionBoard.Show(m);
                    cautionPanel.Show(m);
                };
            }
            else if (cautionBoard)
            {
                cautionBoard.Setup(origin);
                return cautionBoard.Show;
            }
            else if(cautionPanel)
            {
                return cautionPanel.Show;
            }
            else
            {
                return null;
            }
        }

        public Action<string, float> GetMessageHandler(DiagnosticsMessageType type, Optional<bool> isHMD, GameObject origin)
        {
            Setup(type, isHMD);
            if (messageBoard && messagePanel)
            {
                return (m, t) =>
                {
                    messageBoard.Setup(origin);
                    messageBoard.Enqueue(type, m, t);
                    messagePanel.Enqueue(type, m, t);
                };
            }
            else if (messageBoard)
            {
                return (m, t) =>
                {
                    messageBoard.Setup(origin);
                    messageBoard.Enqueue(type, m, t);
                };
            }
            else if(messagePanel)
            {
                return (m, t) =>
                {
                    messagePanel.Enqueue(type, m, t);
                };
            }
            else
            {
                return null;
            }
        }

        public Action<string> GetDumpHandler(Optional<bool> isHMD)
        {
            Setup(DiagnosticsMessageType.Dump, isHMD);
            if (dumpBoard && dumpPanel)
            {
                return (m) =>
                {
                    dumpBoard.Show(m);
                    dumpPanel.Show(m);
                };
            }
            else if (dumpBoard)
            {
                return dumpBoard.Show;
            }
            else if(dumpPanel)
            {
                return dumpPanel.Show;
            }
            else
            {
                return null;
            }
        }

        public Action GetMessageCleaner(DiagnosticsMessageType type, Optional<bool> isHMD)
        {
            Setup(type, isHMD);
            if (messageBoard && messagePanel)
            {
                return () =>
                {
                    messageBoard.DequeueAll();
                    messagePanel.DequeueAll();
                };
            }
            else if (messageBoard)
            {
                return () =>
                {
                    messageBoard.DequeueAll();
                };
            }
            else if(messagePanel)
            {
                return () =>
                {
                    messagePanel.DequeueAll();
                };
            }
            else
            {
                return null;
            }
        }

        public Action GetCautionCleaner(Optional<bool> isHMD)
        {
            Setup(DiagnosticsMessageType.Caution, isHMD);
            if (cautionBoard && cautionPanel)
            {
                return () =>
                {
                    cautionBoard.Hide();
                    cautionPanel.Hide();
                };
            }
            else if (cautionBoard)
            {
                return cautionBoard.Hide;
            }
            else if(cautionPanel)
            {
                return cautionPanel.Hide;
            }
            else
            {
                return null;
            }
        }

        private void Setup(DiagnosticsMessageType type, Optional<bool> isHMD)
        {
            switch (type)
            {
                case DiagnosticsMessageType.Caution:
                    if (isHMD.OnNone)
                    {
                        if (!Application.isEditor && !cautionBoard) { cautionBoard = gameObject.AddComponent<CautionBoard>(); }
                        if (!cautionPanel) { cautionPanel = gameObject.AddComponent<CautionPanel>(); }
                    }
                    else if (isHMD.Value)
                    {
                        if (!cautionBoard) { cautionBoard = gameObject.AddComponent<CautionBoard>(); }
                        if (cautionPanel) { Destroy(cautionPanel); }
                    }
                    else
                    {
                        if (cautionBoard) { Destroy(cautionBoard); }
                        if (!cautionPanel) { cautionPanel = gameObject.AddComponent<CautionPanel>(); }
                    }
                    break;
                case DiagnosticsMessageType.FatalError:
                case DiagnosticsMessageType.Error:
                case DiagnosticsMessageType.Warning:
                case DiagnosticsMessageType.Info:
                    if (isHMD.OnNone)
                    {
                        if (!Application.isEditor && !messageBoard) { messageBoard = gameObject.AddComponent<MessageBoard>(); }
                        if (!messagePanel) { messagePanel = gameObject.AddComponent<MessagePanel>(); }
                    }
                    else if (isHMD.Value)
                    {
                        if (!messageBoard) { messageBoard = gameObject.AddComponent<MessageBoard>(); }
                        if (messagePanel) { Destroy(messagePanel); }
                    }
                    else
                    {
                        if (messageBoard) { Destroy(messageBoard); }
                        if (!messagePanel) { messagePanel = gameObject.AddComponent<MessagePanel>(); }
                    }
                    break;
                case DiagnosticsMessageType.Dump:
                    if (isHMD.OnNone)
                    {
                        if (!dumpPanel) { dumpPanel = gameObject.AddComponent<DumpPanel>(); }
                    }
                    else if (isHMD.Value)
                    {
                        if (!dumpBoard) { dumpBoard = gameObject.AddComponent<DumpBoard>(); }
                        if (dumpPanel) { Destroy(dumpPanel); }
                    }
                    else
                    {
                        if (dumpBoard) { Destroy(dumpBoard); }
                        if (!dumpPanel) { dumpPanel = gameObject.AddComponent<DumpPanel>(); }
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
