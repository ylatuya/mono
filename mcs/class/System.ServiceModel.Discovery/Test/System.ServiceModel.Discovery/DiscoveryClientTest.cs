//
// Author: Atsushi Enomoto <atsushi@ximian.com>
//
// Copyright (C) 2010 Novell, Inc (http://www.novell.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Discovery;
using System.ServiceModel.Dispatcher;
using System.Xml;
using MonoTests.Helpers;
using NUnit.Framework;
using System.Text;

namespace MonoTests.System.ServiceModel.Discovery
{
	[TestFixture]
	public class DiscoveryClientTest
	{
		[Test]
		public void ContractInterfaceManaged()
		{
			var client = new DiscoveryClient(new DiscoveryEndpoint());
			var v11 = client.ChannelFactory.Endpoint;
			Assert.IsNotNull(v11, "v11");
			Assert.AreEqual("DiscoveryProxy", v11.Name, "v11.Name");
			Assert.AreEqual(2, v11.Contract.Operations.Count, "v11.Operations.Count");
			Assert.IsNull(v11.Contract.CallbackContractType, "v11.CallbackContractType");
		}

		[Test]
		public void ContractInterfaceAdhoc()
		{
			var client = new DiscoveryClient(new UdpDiscoveryEndpoint());
			var v11 = client.ChannelFactory.Endpoint;
			var cd = ContractDescription.GetContract(v11.Contract.ContractType);
			Assert.IsNotNull(v11, "v11");
			Assert.AreEqual("CustomBinding_TargetService", v11.Name, "v11.Name");
			Assert.AreEqual(5, v11.Contract.Operations.Count, "v11.Operations.Count");
			Assert.IsNotNull(v11.Contract.CallbackContractType, "v11.CallbackContractType");
		}

		[Test]
		public void TestClientDiscovery()
		{
			/*UdpClient server = new UdpClient(3702);
			server.JoinMulticastGroup(IPAddress.Parse("239.255.255.250"));
			IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
			server.ReceiveAsync().ContinueWith(r =>
			{
				string response = @"<?xml version='1.0' encoding='utf-8'?>
					<s:Envelope xmlns:a='http://schemas.xmlsoap.org/ws/2004/08/addressing' xmlns:s='http://www.w3.org/2003/05/soap-envelope'>
						<s:Header>
							<a:Action s:mustUnderstand='1'>http://schemas.xmlsoap.org/ws/2005/04/discovery/Probe</a:Action>
							<a:MessageID>urn:uuid:e77dfa98-1cad-4520-9f1d-2a5c561dcbb8</a:MessageID>
						</s:Header>
						<s:Body>
							<Probe xmlns='http://schemas.xmlsoap.org/ws/2005/04/discovery'>
								<Duration xmlns='http://schemas.microsoft.com/ws/2008/06/discovery'>PT5S</Duration>
							</Probe>
						</s:Body>
					</s:Envelope>";
				var bytes = Encoding.ASCII.GetBytes(response);
				server.Send(bytes, bytes.Length, r.Result.RemoteEndPoint);
				Console.WriteLine(response);
			});
			*/

			UdpDiscoveryEndpoint ude = new UdpDiscoveryEndpoint(DiscoveryVersion.WSDiscoveryApril2005);
			DiscoveryClient discoveryClient = new DiscoveryClient(ude);
			FindCriteria findCriteria = new FindCriteria();
			findCriteria.ContractTypeNames.Add(new XmlQualifiedName("NetworkVideoTransmitter", @"http://www.onvif.org/ver10/network/wsdl"));
			findCriteria.MaxResults = 1;
			findCriteria.Duration = TimeSpan.FromSeconds(5);
			discoveryClient.Find(findCriteria);
			//server.Close();
		}
	}
}
