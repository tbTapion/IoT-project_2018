using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessagePair{

	public string topic, payload;

	public MessagePair(string topic, string payload){
		this.topic = topic;
		this.payload = payload;
	}
}
