using System;
using System.Threading.Tasks;

using Lab1;

class Program{
   static async Task Main(string[] args){
      if(args.Length==0){
         Console.WriteLine("Please enter the parameter: send or receive");
         return;
      }

      if(args[0] == "send"){
         await Sender.SendMessage();
      } else if (args[0] == "receive"){
         await Receiver.ReceiveMessage();
      } else {
         Console.WriteLine("Parameter is not valid. Use send or receive");
      }
   }
}