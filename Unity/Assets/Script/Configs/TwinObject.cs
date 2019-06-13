using System;
using System.Collections;
using System.Collections.Generic;
using ExactFramework.Component;
using ExactFramework.Handlers;
using UnityEngine;
using UnityEngine.Events;

namespace ExactFramework.Configuration{

    ///<summary>
    ///Digital Twin object class used as a base for all Digital Twin tyoe objects. Can have multiple components connected to it. 
    ///Serves as the base for the digital representation of a physical object and holds a reference to the MQTT message handler.
    ///</summary>
    public abstract class TwinObject : MonoBehaviour
    {

        //MQTT variables
        ///<summary>
        ///Reference variable to the MQTT Handler for MQTT messages.
        ///</summary>
        private MQTTHandler mqttHandler;
        ///<summary>
        /// The ID of the physical device. Its MAC-address.
        ///</summary>
        public string deviceID;
        ///<summary>
        ///Link bool for the link between Digital Twin Object(this) and physical device.
        ///</summary>
        public bool linked;
        ///<summary>
        ///Configuration name/ID set in Unity and on the physical device to be used when linking the digital object to a physical one.
        ///</summary>
        protected string configName;

        //Variables used for a simple ping operation
        private int pingCount;
        private int pingTime;

        private string name; //unused name variable.

        ///<summary>
        ///Buffer for message of the action type. New messages are added from the components. Used by the MQTTHandler to build one multi message to the device.
        ///</summary>
        public List<MessagePair> actionMsgBuffer = new List<MessagePair>(); 
        ///<summary>
        ///Buffer for message of the get type. New messages are added from the components. Used by the MQTTHandler to build one multi message to the device.
        ///</summary>
        public List<MessagePair> getMsgBuffer = new List<MessagePair>();
        ///<summary>
        ///Dictionary for the DeviceComponent type components added to the twin object.
        ///</summary>
        private Dictionary<string, DeviceComponent> deviceComponents = new Dictionary<string, DeviceComponent>();
        ///<summary>
        ///Dictionary for events.
        ///</summary>
        private Dictionary<string, UnityEvent> eventList = new Dictionary<string, UnityEvent>();

        // Use this for initialization
        protected virtual void Start()
        {
            pingCount = 0;
            pingTime = 60 * 60;
        }

        // Update is called once per frame. Holds the simple ping messaging handling.
        protected virtual void Update()
        {
            if (linked)
            {
                pingTime--;
                if (pingTime == 0)
                {
                    if (pingCount > 2)
                    {
                        linked = false;
                        Debug.Log("Tile " + deviceID + " disconnected...");
                    }
                    else
                    {
                        SendPingMessage();
                        pingCount++;
                    }
                    pingTime = 60 * 60;
                }
            }
        }

        ///<summary>
        ///Sets the reference to the MQTTHandler object class. 
        ///</summary>
        ///<param name="mqttHandler">MQTTHandler object reference.</param>
        public void SetMQTTHandler(MQTTHandler mqttHandler)
        {
            this.mqttHandler = mqttHandler;
        }
        
        ///<summary>
        ///Sends a message to the MQTTHandler to be sent to a device. Takes the topic and a string payload.
        ///Payload is converted from string to a byte array.
        ///</summary>
        ///<param name="tempMessageTopic">Message topic variable to send to the physical device.</param>
        ///<param name="payload">MQTT message payload to be sent to the physical device</param>
        internal void SendDeviceMessage(string tempMessageTopic, string payload)
        {
            if (mqttHandler != null)
            {
                mqttHandler.SendDeviceMessage(tempMessageTopic, System.Text.Encoding.Default.GetBytes(payload));
            }
        }

        ///<summary>
        ///Sends a message to the MQTTHandler to be sent to a device. Takes the topic and an int payload.
        ///Payload is converted from integer to a byte array.
        ///</summary>
        ///<param name="tempMessageTopic">Message topic variable to send to the physical device.</param>
        ///<param name="payload">MQTT message payload to be sent to the physical device.</param>
        internal void SendDeviceMessage(string tempMessageTopic, int payload)
        {
            if (mqttHandler != null)
            {
                mqttHandler.SendDeviceMessage(tempMessageTopic, new byte[] {(byte)payload });
            }
        }

        internal void SendDeviceMessage(string tempMessageTopic, int[] payload)
        {

        }

        ///<summary>
        ///Sends a message to the MQTTHandler to be sent to a device. Takes the topic and a bool payload.
        ///Payload is converted from bool to a byte array.
        ///</summary>
        ///<param name="tempMessageTopic">Message topic variable to send to the physical device.</param>
        ///<param name="payload">MQTT message payload to be sent to the physical device.</param>
        internal void SendDeviceMessage(string tempMessageTopic, bool payload)
        {
            if (mqttHandler != null)
            {
                if(payload == true){
                    mqttHandler.SendDeviceMessage(tempMessageTopic, new byte[] {(byte)1});
                }else{
                    mqttHandler.SendDeviceMessage(tempMessageTopic, new byte[] {(byte)0});
                }
            }
        }

        ///<summary>
        ///Adds an action message to the action message buffer. Takes the name of the component to be updated and a string payload.
        ///Payload is converted from string to a byte array.
        ///Payload has an added variable in front for the payload length.
        ///</summary>
        ///<param name="componentName">Name of the component to be updated.</param>
        ///<param name="payload">MQTT message payload to be sent to the physical device.</param>
        public void SendActionMessage(string componentName, string payload)
        {
            //sendDeviceMessage(deviceID + "/action/" + componentName, payload);
            List<byte> temp = new List<byte>();
            temp.Add((byte)payload.Length);
            temp.AddRange(System.Text.Encoding.Default.GetBytes(payload));
            AddActionMessageToBuffer(componentName, temp.ToArray());
        }

        ///<summary>
        ///Adds an action message to the action message buffer. Takes the name of the component to be updated and an int payload.
        ///Payload is converted from int to a byte array.
        ///Payload has an added variable in front for the payload length.
        ///</summary>
        ///<param name="componentName">Name of the component to be updated.</param>
        ///<param name="payload">MQTT message payload to be sent to the physical device.</param>
        public void SendActionMessage(string componentName, int payload)
        {
            //sendDeviceMessage(deviceID + "/action/" + componentName, payload);
            AddActionMessageToBuffer(componentName, new byte[] {1, (byte)payload });
        }

        ///<summary>
        ///Adds an action message to the action message buffer. Takes the name of the component to be updated and a bool payload.
        ///Payload is converted from bool to a byte array. 
        ///Payload has an added variable in front for the payload length.
        ///</summary>
        ///<param name="componentName">Name of the component to be updated.</param>
        ///<param name="payload">MQTT message payload to be sent to the physical device.</param>
        public void SendActionMessage(string componentName, bool payload)
        {
            if (mqttHandler != null)
            {
                if(payload == true){
                    AddActionMessageToBuffer(componentName, new byte[] {1, (byte)1});
                }else{
                    AddActionMessageToBuffer(componentName, new byte[] {1,  (byte)0});
                }
            }
            //sendDeviceMessage(deviceID + "/action/" + componentName, payload);
        }

        ///<summary>
        ///Adds an action message to the action message buffer. Takes the name of the component to be updated and a byte array payload.
        ///Payload has an added variable in front for the payload length.
        ///</summary>
        ///<param name="componentName">Name of the component to be updated.</param>
        ///<param name="payload">MQTT message payload to be sent to the physical device.</param>
        public void SendActionMessage(string componentName, byte[] payload)
        {
            if (mqttHandler != null)
            {
                //mqttHandler.sendDeviceMessage(deviceID + "/action/" + componentName, payload);
                List<byte> temp = new List<byte>();
                temp.Add((byte)payload.Length);
                temp.AddRange(payload);
                AddActionMessageToBuffer(componentName, temp.ToArray());
            }
        }

        ///<summary>
        ///The method called by all the SendActionMessage methods. 
        ///The message is put into a message pair type object which is then added to the action message buffer.
        ///Buffer is later used by the MQTTHandler to build message and send them.
        ///</summary>
        ///<param name="componentName">Name of the component to be updated.</param>
        ///<param name="payload">MQTT message payload to be sent to the physical device.</param>
        private void AddActionMessageToBuffer(string componentName, byte[] payload){
            actionMsgBuffer.Add(new MessagePair(componentName, payload));
        }

        ///<summary>
        ///Ping message method.
        ///Calls the SendDeviceMessage method with the device and message type, and the bool true for the payload.
        ///</summary>
        public void SendPingMessage()
        {
            SendDeviceMessage(deviceID + "/ping", true);
        }

        ///<summary>
        ///Adds a get message to the get message buffer. Get message is based on the component's name. 
        ///The component name and an empty byte array are added to a message pair object which is added to the buffer.
        ///</summary>
        public void SendGetMessage(string componentName)
        {
            //sendDeviceMessage(deviceID + "/get/" + componentName, 1);
            if(mqttHandler != null){
                getMsgBuffer.Add(new MessagePair(componentName, new byte[]{0}));
            }
        }

        ///<summary>
        ///Get config message method.
        ///Not used.
        ///Sends a get config message to the device. Would have returned the configuration ID of the physical object.
        ///</summary>
        public void SendGetConfigMessage()
        {
            SendDeviceMessage(deviceID + "/getconfg", 1);
        }

        ///<summary>
        ///Gets the device ID of the object. This is the MAC-address of the physical object.
        ///</summary>
        ///<returns>A Device ID string is returned.</returns>
        public string GetDeviceID()
        {
            return deviceID;
        }

        ///<summary>
        ///Returns the status of whether this TwinObject is connected to a physical device.
        ///<summary>
        ///<returns>Bool of linked status.</returns>
        public bool GetLinkStatus()
        {
            return linked;
        }

        ///<summary>
        ///Method to set the linked status of this TwinObject. 
        ///</summary>
        ///<param name="linked">Bool to set for the linked status.</param>
        public void SetLinkStatus(bool linked)
        {
            this.linked = linked;
        }

        ///<summary>
        ///Method to set the linked status of this TwinObject and the device ID of the physical object.
        ///Called by the MQTTHandler when a new device connects that matches this twin object's configuration ID.
        ///Sends a ping message in return to the physical device to let it know that it has been successfully linked.
        ///</summary>
        ///<param name="deviceID">MAC-address of the physical device.</param>
        public void LinkDevice(string deviceID)
        {
            this.deviceID = deviceID;
            SetLinkStatus(true);
            SendPingMessage();
        }

        ///<summary>
        ///Method to get the configuration name of this digital twin object.
        ///</summary>
        ///<returns>Configuration name</returns>
        public string GetConfigName()
        {
            return configName;
        }

        ///<summary>
        ///Method called by the MQTTHandler when an Event type message is received from the physical device.
        ///Resets the ping message timer. Sets temporary component name and event type variables. 
        ///A component has its UpdateComponent method called based on the component's name.
        ///The event type and payload are sent to the component to update the component accordingly
        ///</summary>
        ///<param name="topic">Topic of the MQTT event message received.</param>
        ///<param name="payload">Payload of the MQTT event message received.</param>
        public virtual void EventMessage(string[] topic, byte[] payload)
        {   
            pingTime = 60*60;
            //EventMessage msg = new EventMessage(topic[4], topic[5], payload);
            //UpdateComponent(msg);
            //OnEvent(msg);
            string componentname = topic[4];
            string eventType = topic[5];
            deviceComponents[componentname].UpdateComponent(eventType, payload);
        }

        ///<summary>
        ///Method called by the MQTTHandler when a Value type message is received from the physical device. 
        ///Value type messages are responses from the Get type messages sent from the twin objects.
        ///Resets the ping message timer. Sets temporary component name and event type variables.
        ///Event type variable here, as opposed to the EventMessage method, is just what part of the component's data has been received.
        ///A component has its UpdateComponent method called based on the component's name.
        ///The event type and payload are sent to the component to update the component accordingly.
        ///<param name="topic">Topic of the MQTT value message received.</param>
        ///<param name="payload">Payload of the MQTT value message received.</param>
        public virtual void ValueMessage(string[] topic, byte[] payload)
        {
            pingTime = 60*60;
            //EventMessage msg = new EventMessage(topic[4], topic[5], payload);
            //UpdateComponent(new EventMessage(topic[4], topic[5], payload));
            string componentname = topic[4];
            string eventType = topic[5];
            deviceComponents[componentname].UpdateComponent(eventType, payload);
        }

        ///<summary>
        ///Method called when the MQTTHandler receives a ping message from the physical device linked to this twin object.
        ///Resets the ping count variable.
        ///</summary>
        public void PingResponse()
        {
            pingCount = 0;
        }

        /// <summary>
        /// Sets the name of the twin object. Not necessarily used for anything.
        /// </summary>
        public void SetName(string name){
            if(transform != null){
                transform.name = name;
            }
            this.name = name;
        }

        //protected virtual void OnEvent(EventMessage e) { }

        //protected abstract void UpdateComponent(EventMessage e);

        /// <summary>
        /// Creates and adds a component to the device component dictionary of this twin object.
        /// Generics type method. Uses a DeviceComponent type object.
        /// </summary>
        /// <typeparam name="T">DeviceComponent</typeparam>
        /// <returns>A DeviceComponent object created from the generics type.</returns>
        /// <param name="id">The ID set on the component added.</param>
        public T AddDeviceComponent<T>(string id) where T : DeviceComponent{
            T component = gameObject.AddComponent<T>();
            deviceComponents.Add(id, component);
            component.SetComponentID(id);
            return component;
        }

        /// <summary>
        /// Returns a DeviceComponent type object based on the generics type and id provided.
        /// Throws an error if the component retrieved with the ID doesn't match the generics type.
        /// </summary>
        ///<typeparam name="T">DeviceComponent</typeparam>
        /// <returns>A DeviceComponent object based on the id and generics type.</returns>
        /// <param name="id">The ID of the component requested.</param>
        public T GetDeviceComponent<T>(string id) where T : DeviceComponent{
            try{
                T component = (T)deviceComponents[id];
                return component;
            }catch(Exception e){
                Debug.Log(e.StackTrace);
            }
            return null;
        }

        ///<summary>
        /// Creates and adds a component to the device component dictionary of this twin object based on the generics type provided.
        /// ID used in the dictionary is based on the type name of the device component class object.
        ///</summary>
        ///<typeparam name="T">DeviceComponent</typeparam>
        ///<returns>A DeviceComponent object created from the generics type.</returns>
        public T AddDeviceComponent<T>() where T : DeviceComponent{
            T component = gameObject.AddComponent<T>();
            string id = component.GetType().Name.ToLower();
            deviceComponents.Add(id, component);
            return component;
        }

        ///<summary>
        /// Returns a DeviceComponent type object based on the generics type.
        /// Throws an error if no component exists. ID used is generated from the type name of the generics type.
        /// Useful for single instances of a DeviceComponent type.
        ///</summary>
        ///<typeparam name="T">DeviceComponent</typeparam>
        ///<returns>A device component object based on the generics type.</returns>
        public T GetDeviceComponent<T>() where T : DeviceComponent{
            try{
                string id = typeof(T).Name.ToLower();
                T component = (T)deviceComponents[id];
                return component;
            }catch(Exception e){
                Debug.Log(e.StackTrace);
            }
            return null;
        }

        ///<summary>
        ///Adds an existing DeviceComponent object to the the dictionary. The ID is created from the object type's name.
        ///</summary>
        ///<param name="component">The DeviceComponent to be added.</param>
        protected void AddExistingDeviceComponent(DeviceComponent component){
            string id = component.GetType().Name.ToLower();
            deviceComponents.Add(id, component);
        }
        
        ///<summary>
        ///Adds an existing DeviceComponent object to the dictionary with the ID and object supplied.
        ///</summary>
        ///<param name="id">The ID of the component added.</param>
        ///<param name="component">The DeviceComponent to be added.</param>
        protected void AddExistingDeviceComponent(string id, DeviceComponent component){
            deviceComponents.Add(id, component);
        }

        ///<summary>
        ///Adds a unity action to the event listener list. If no event with the name exists, add a new event and add the action to the listener.
        ///</summary>
        ///<param name="eventName">Name of the event to use when invoking the event.</param>
        ///<param name="action">Unity action to add to the event.</param>
        public void AddEventListener(string eventName, UnityAction action){
            if(eventList.ContainsKey(eventName)){
                eventList[eventName].AddListener(action);
            }else{
                UnityEvent tempEvent = new UnityEvent();
                tempEvent.AddListener(action);
                eventList.Add(eventName, tempEvent);
            }
        }

        ///<summary>
        ///Invokes the event with the given name if it exists in the event list.
        ///</summary>
        ///<param name="eventName">Name of the event to invoke.</param>
        public void InvokeEvent(string eventName){
            if(eventList.ContainsKey(eventName)){
                eventList[eventName].Invoke();
            }
        }

    }
}
