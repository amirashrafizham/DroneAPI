using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace DroneAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DroneController : ControllerBase
    {

        [HttpGet("connect")]
        public ActionResult Connect()
        {
            return Ok(DroneCommand("command"));
        }


        [HttpGet("battery")]
        public ActionResult GetBattery()
        {
            return Ok(DroneCommand("battery?"));
        }


        [HttpGet("takeoff")]
        public ActionResult TakeOff()
        {
            return Ok(DroneCommand("takeoff"));
        }

        [HttpGet("land")]
        public ActionResult Land()
        {
            return Ok(DroneCommand("land"));
        }

        [HttpGet("emergency")]
        public ActionResult Emergency()
        {
            return Ok(DroneCommand("emergency"));
        }

        public static string DroneCommand(string command)
        {

            UdpClient udpClient = new UdpClient(8889);
            try
            {
                udpClient.Connect("192.168.10.1", 8889);

                // Sends a message to the host to which you have conected.
                Byte[] sendBytes = Encoding.ASCII.GetBytes(command);

                udpClient.Send(sendBytes, sendBytes.Length);

                //IPEndPoint object will allow us to read datagrams sent from any source.
                IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

                // Blocks until a message returns on this socket from a remote host.
                Byte[] receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);
                string returnData = Encoding.ASCII.GetString(receiveBytes);

                // Uses the IPEndPoint object to determine which of these two hosts responded.
                Console.WriteLine("This is the message you received " +
                                             returnData.ToString());
                Console.WriteLine("This message was sent from " +
                                            RemoteIpEndPoint.Address.ToString() +
                                            " on their port number " +
                                            RemoteIpEndPoint.Port.ToString());
                udpClient.Close();
                return returnData.ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return e.ToString();
            }
        }

    }
}