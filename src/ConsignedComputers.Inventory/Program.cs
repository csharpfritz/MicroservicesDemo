﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microphone;
using Microphone.AspNet;

namespace ConsignedComputers.Inventory
{
  public class Program
  {
    public static void Main(string[] args)
    {

      var host = new WebHostBuilder()
          .UseKestrel()
          .UseContentRoot(Directory.GetCurrentDirectory())
          .UseStartup<Startup>()
          .Build();

      host.Run();
    }
  }
}
