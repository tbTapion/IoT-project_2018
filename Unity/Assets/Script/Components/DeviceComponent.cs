using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExactFramework.Configuration;

namespace ExactFramework.Component {
    ///<summary>
    /// A device component class representing a physical component connected to the device.
    ///</summary>
    [RequireComponent(typeof(TwinObject))]
    public abstract class DeviceComponent : MonoBehaviour
    {
        ///<summary>
        ///Twin object variable reference. Used by component to call message sending, etc.
        ///</summary>
        protected TwinObject device;

        ///<summary>
        ///ID of the component used for message sending and updates.
        ///</summary>
        protected string id;

        //Unity's Start method called before the first frame update.
        public virtual void Start()
        {
            device = GetComponent<TwinObject>();
        }

        ///<summary>
        ///Sets the component's ID to be used for message building and updating the component on incoming messages.
        ///</summary>
        ///<param name="id">ID/name of the component.</param>
        public void SetComponentID(string id){
            this.id = id;
        }

        ///<summary>
        ///Called by the twin object on incoming messages to update the component. 
        ///Set to virtual instead of abstract as all components do not need this method.
        ///</summary>
        ///<param name="eventType">Name of the event type on the component.</param>
        ///<param name="payload">Payload of the MQTT message</param>
        public virtual void UpdateComponent(string eventType, byte[] payload){}
    }
}