using R2API.Networking.Interfaces;
using RoR2;
using UnityEngine.Networking;
using UnityEngine;

namespace OfficialScoutMod.Scout.Components
{
    internal class SyncAtomic : INetMessage
    {
        private NetworkInstanceId netId;
        private ulong gauge;

        public SyncAtomic()
        {
        }

        public SyncAtomic(NetworkInstanceId netId, ulong gauge)
        {
            this.netId = netId;
            this.gauge = gauge;
        }

        public void Deserialize(NetworkReader reader)
        {
            this.netId = reader.ReadNetworkId();
            this.gauge = reader.ReadUInt64();
        }

        public void OnReceived()
        {
            GameObject bodyObject = Util.FindNetworkObject(this.netId);
            if (!bodyObject)
            {
                Log.Message("No Body Object");
                return;
            }

            ScoutController scoutController = bodyObject.GetComponent<ScoutController>();

            if (scoutController)
            {
                scoutController.atomicGauge = this.gauge * 0.01f;

            }
        }

        public void Serialize(NetworkWriter writer)
        {
            writer.Write(this.netId);
            writer.Write(this.gauge);
        }
    }
}
