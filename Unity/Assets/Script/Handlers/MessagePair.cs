
namespace ExactFramework.Handlers
{
    /*
    Class used by the MQTTHandler to hold data on the incoming messages.
     */
    public class MessagePair
    {

        public string topic; //Topic of the incoming message from MQTT.
        public byte[] payload; //Payload of the incoming message from MQTT.

        /*
        Constructor. Sets the topic and payload variables of this object from the MQTT message.

        Parameter type: string, byte[]
        Parameter: topic, payload
         */
        public MessagePair(string topic, byte[] payload)
        {
            this.topic = topic;
            this.payload = payload;
        }
    }
}
