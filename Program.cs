// Decompiled with JetBrains decompiler
// Type: TestIISCConsole.Program
// Assembly: TestIISCConsole, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 97A5FE6B-60BB-49F5-AF9A-D41F242EA9EB
// Assembly location: I:\Projects\VS2013\TestIISCConsole\bin\Debug\TestIISCConsole.exe

using Microsoft.Web.Administration;
using System;
 using System.Collections.Generic;
using System.IO;

namespace TestIISCConsole
{
    internal class Program
    {
        //Path of the IIS Dev projects
        private static string pathDevDirectory = @"C:\Developpement\";

        private static void Main(string[] args)
        {
            Console.WriteLine("Enter the suffix version (ex: 700) to create the CConsoleXXX version:");
            string versionNumber = Console.ReadLine();

            //Console.WriteLine("Is Comple[o]Console folder name ? [y/n]");
            //string cconsoleName = Console.ReadLine().ToString() == "y" ? "CompleoConsole" : "CompleConsole";
            string cconsoleName = "CompleoConsole";


            Console.WriteLine("Create Console or Websign site ? [c (console)/w (websign)]");
            if (Console.ReadLine() == "c")
                Program.CreateCConsoleWebsiteForIIS(versionNumber, cconsoleName);
            else
                Program.CreateCConsoleWebSignSiteForIIS(versionNumber, cconsoleName);
        }

        private static void CreateCConsoleWebSignSiteForIIS(string versionNumber, string cconsoleName)
        {
            try
            {
                string str1 = pathDevDirectory + cconsoleName + "\\version-v" + versionNumber + "\\";
                string name = "CCWebSign" + versionNumber;
                string index = name;
                string str2 = "/";
                string path = str1 + "CmpWebSignRoot";
                Directory.CreateDirectory(path);
                string str3 = "*";
                string str4 = (int.Parse(versionNumber = versionNumber.Replace(".", "")) + 5).ToString();
                string str5 = "";
                string str6 = "/";
                long num1 = 1;
                Dictionary<string, string> dictionary = new Dictionary<string, string>()
        {
          {
            "/cmpwebsign",
            str1 + "CmpWebSign"
          }
        };
                using (ServerManager serverManager = new ServerManager())
                {
                    if (serverManager.Sites[index] != null)
                    {
                        Program.w("WebSite: " + index + " already exists!");
                    }
                    else
                    {
                        ApplicationPool applicationPool = serverManager.ApplicationPools.Add(name);
                        applicationPool.ManagedRuntimeVersion = "v4.0";
                        applicationPool.ManagedPipelineMode = ManagedPipelineMode.Integrated;
                        applicationPool.Enable32BitAppOnWin64 = true;
                        applicationPool.ProcessModel.IdentityType = ProcessModelIdentityType.NetworkService;
                        serverManager.CommitChanges();
                        foreach (Site site in (ConfigurationElementCollectionBase<Site>)serverManager.Sites)
                        {
                            if (site.Id > num1)
                                num1 = site.Id;
                        }
                        long num2 = num1 + 1L;
                        Site element1 = serverManager.Sites.CreateElement();
                        element1.SetAttributeValue("name", (object)index);
                        element1.Id = num2;
                        element1.Bindings.Clear();
                        string str7 = str3 + ":" + str4 + ":" + str5;
                        Binding element2 = element1.Bindings.CreateElement();
                        element2.Protocol = "http";
                        element2.BindingInformation = str7;
                        element1.Bindings.Add(element2);
                        Application element3 = element1.Applications.CreateElement();
                        element3.Path = str6;
                        element3.ApplicationPoolName = name;
                        VirtualDirectory element4 = element3.VirtualDirectories.CreateElement();
                        element4.Path = str2;
                        element4.PhysicalPath = path;
                        element3.VirtualDirectories.Add(element4);
                        element1.Applications.Add(element3);
                        serverManager.Sites.Add(element1);
                        foreach (KeyValuePair<string, string> keyValuePair in dictionary)
                        {
                            string key = keyValuePair.Key;
                            string str8 = keyValuePair.Value;
                            Application element5 = element1.Applications.CreateElement();
                            element5.Path = key;
                            element5.ApplicationPoolName = name;
                            VirtualDirectory element6 = element5.VirtualDirectories.CreateElement();
                            element6.Path = "/";
                            element6.PhysicalPath = str8;
                            element5.VirtualDirectories.Add(element6);
                            Configuration hostConfiguration = serverManager.GetApplicationHostConfiguration();
                            string str9 = "system.webServer/security/authentication/";
                            if (key == "/cmpwebsign")
                            {
                                ConfigurationSection section = hostConfiguration.GetSection(str9 + "anonymousAuthentication", index + key);
                                section.OverrideMode = OverrideMode.Allow;
                                section["enabled"] = (object)true;
                            }
                            element1.Applications.Add(element5);
                        }
                        serverManager.CommitChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                Program.w(ex.Message + (ex.InnerException != null ? ex.InnerException.Message : ""));
            }
        }

        private static void CreateCConsoleWebsiteForIIS(string versionNumber, string cconsoleName)
        {
            try
            {
                string str1 = pathDevDirectory + cconsoleName + "\\version-v" + versionNumber + "\\";
                string name = "CCVersion" + versionNumber;
                string index = name;
                string str2 = "/";
                string path = str1 + "CConsoleRoot";
                Directory.CreateDirectory(path);
                string str3 = "*";
                string str4 = int.Parse(versionNumber = versionNumber.Replace(".", "")).ToString();
                string str5 = "";
                string str6 = "/";
                long num1 = 1;
                Dictionary<string, string> dictionary = new Dictionary<string, string>()
                {
                  {
                    "/ccmobile",
                    str1 + "CCMobile"
                  },
                  {
                    "/cconsole",
                    str1 + "CConsole"
                  },
                  {
                    "/cconsolesso",
                    str1 + "CConsoleSSO"
                  },
                  {
                    "/ccwebapi",
                    str1 + "WebApiServer"
                  },
                  {
                    "/cmparch",
                    str1 + "CmpArch"
                  },
                  {
                    "/cmparchintra",
                    str1 + "CmpArchIntranet"
                  }
                };

                using (ServerManager serverManager = new ServerManager())
                {
                    if (serverManager.Sites[index] != null)
                    {
                        Program.w("WebSite: " + index + " already exists!");
                    }
                    else
                    {
                        ApplicationPool applicationPool = serverManager.ApplicationPools.Add(name);
                        applicationPool.ManagedRuntimeVersion = "v4.0";
                        applicationPool.ManagedPipelineMode = ManagedPipelineMode.Integrated;
                        applicationPool.Enable32BitAppOnWin64 = true;
                        applicationPool.ProcessModel.IdentityType = ProcessModelIdentityType.NetworkService;
                        serverManager.CommitChanges();
                        foreach (Site site in (ConfigurationElementCollectionBase<Site>)serverManager.Sites)
                        {
                            if (site.Id > num1)
                                num1 = site.Id;
                        }
                        long num2 = num1 + 1L;
                        Site element1 = serverManager.Sites.CreateElement();
                        element1.SetAttributeValue("name", (object)index);
                        element1.Id = num2;
                        element1.Bindings.Clear();
                        string str7 = str3 + ":" + str4 + ":" + str5;
                        Binding element2 = element1.Bindings.CreateElement();
                        element2.Protocol = "http";
                        element2.BindingInformation = str7;
                        element1.Bindings.Add(element2);
                        Application element3 = element1.Applications.CreateElement();
                        element3.Path = str6;
                        element3.ApplicationPoolName = name;
                        VirtualDirectory element4 = element3.VirtualDirectories.CreateElement();
                        element4.Path = str2;
                        element4.PhysicalPath = path;
                        element3.VirtualDirectories.Add(element4);
                        element1.Applications.Add(element3);
                        serverManager.Sites.Add(element1);
                        foreach (KeyValuePair<string, string> keyValuePair in dictionary)
                        {
                            string key = keyValuePair.Key;
                            string str8 = keyValuePair.Value;
                            Application element5 = element1.Applications.CreateElement();
                            element5.Path = key;
                            element5.ApplicationPoolName = name;
                            VirtualDirectory element6 = element5.VirtualDirectories.CreateElement();
                            element6.Path = "/";
                            element6.PhysicalPath = str8;
                            element5.VirtualDirectories.Add(element6);
                            Configuration hostConfiguration = serverManager.GetApplicationHostConfiguration();
                            string str9 = "system.webServer/security/authentication/";
                            if (key == "/cmparch" || key == "/cconsole" || key == "/ccmobile")
                            {
                                ConfigurationSection section1 = hostConfiguration.GetSection(str9 + "anonymousAuthentication", index + key);
                                section1.OverrideMode = OverrideMode.Allow;
                                section1["enabled"] = (object)true;
                                ConfigurationSection section2 = hostConfiguration.GetSection(str9 + "basicAuthentication", index + key);
                                section2.OverrideMode = OverrideMode.Allow;
                                section2["enabled"] = (object)false;
                                ConfigurationSection section3 = hostConfiguration.GetSection(str9 + "windowsAuthentication", index + key);
                                section3.OverrideMode = OverrideMode.Allow;
                                section3["enabled"] = (object)false;
                            }
                            if (key == "/cconsolesso")
                            {
                                ConfigurationSection section1 = hostConfiguration.GetSection(str9 + "anonymousAuthentication", index + key);
                                section1.OverrideMode = OverrideMode.Allow;
                                section1["enabled"] = (object)false;
                                ConfigurationSection section2 = hostConfiguration.GetSection(str9 + "basicAuthentication", index + key);
                                section2.OverrideMode = OverrideMode.Allow;
                                section2["enabled"] = (object)false;
                                ConfigurationSection section3 = hostConfiguration.GetSection(str9 + "windowsAuthentication", index + key);
                                section3.OverrideMode = OverrideMode.Allow;
                                section3["enabled"] = (object)true;
                            }
                            if (key == "/cmparchintra")
                            {
                                ConfigurationSection section1 = hostConfiguration.GetSection(str9 + "anonymousAuthentication", index + key);
                                section1.OverrideMode = OverrideMode.Allow;
                                section1["enabled"] = (object)true;
                                ConfigurationSection section2 = hostConfiguration.GetSection(str9 + "basicAuthentication", index + key);
                                section2.OverrideMode = OverrideMode.Allow;
                                section2["enabled"] = (object)false;
                                ConfigurationSection section3 = hostConfiguration.GetSection(str9 + "windowsAuthentication", index + key);
                                section3.OverrideMode = OverrideMode.Allow;
                                section3["enabled"] = (object)true;
                            }
                            element1.Applications.Add(element5);
                        }
                        serverManager.CommitChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                Program.w(ex.Message + (ex.InnerException != null ? ex.InnerException.Message : ""));
            }
        }

        private static void CreateCConsoleWebsiteForIIS_OLD()
        {
            try
            {
                Console.WriteLine("Enter the suffix version (ex: 700) to create the CConsoleXXX version:");
                string str1 = Console.ReadLine();
                string str2 = pathDevDirectory + "CompleoConsole\\version -v" + str1 + "\\";
                string name = "CCVersion" + str1;
                string index = name;
                string str3 = "/";
                string str4 = str2 + "CConsoleRoot";
                string str5 = "*";
                string str6 = str1;
                string str7 = "";
                string str8 = "/";
                long num1 = 1;
                Dictionary<string, string> dictionary = new Dictionary<string, string>()
        {
          {
            "/ccmobile",
            str2 + "CCMobile"
          },
          {
            "/cconsole",
            str2 + "CConsole"
          },
          {
            "/cconsolesso",
            str2 + "CConsoleSSO"
          },
          {
            "/ccwebapi",
            str2 + "WebApiServer"
          },
          {
            "/cmparch",
            str2 + "CmpArch"
          },
          {
            "/cmparchintra",
            str2 + "CmpArchIntranet"
          }
        };
                using (ServerManager serverManager = new ServerManager())
                {
                    if (serverManager.Sites[index] != null)
                    {
                        Program.w("WebSite: " + index + " already exists!");
                    }
                    else
                    {
                        ApplicationPool applicationPool = serverManager.ApplicationPools.Add(name);
                        applicationPool.ManagedRuntimeVersion = "v4.0";
                        applicationPool.ManagedPipelineMode = ManagedPipelineMode.Integrated;
                        applicationPool.Enable32BitAppOnWin64 = true;
                        applicationPool.ProcessModel.IdentityType = ProcessModelIdentityType.NetworkService;
                        serverManager.CommitChanges();
                        foreach (Site site in (ConfigurationElementCollectionBase<Site>)serverManager.Sites)
                        {
                            if (site.Id > num1)
                                num1 = site.Id;
                        }
                        long num2 = num1 + 1L;
                        Site element1 = serverManager.Sites.CreateElement();
                        element1.SetAttributeValue("name", (object)index);
                        element1.Id = num2;
                        element1.Bindings.Clear();
                        string str9 = str5 + ":" + str6 + ":" + str7;
                        Binding element2 = element1.Bindings.CreateElement();
                        element2.Protocol = "http";
                        element2.BindingInformation = str9;
                        element1.Bindings.Add(element2);
                        Application element3 = element1.Applications.CreateElement();
                        element3.Path = str8;
                        element3.ApplicationPoolName = name;
                        VirtualDirectory element4 = element3.VirtualDirectories.CreateElement();
                        element4.Path = str3;
                        element4.PhysicalPath = str4;
                        element3.VirtualDirectories.Add(element4);
                        element1.Applications.Add(element3);
                        serverManager.Sites.Add(element1);
                        foreach (KeyValuePair<string, string> keyValuePair in dictionary)
                        {
                            string key = keyValuePair.Key;
                            string str10 = keyValuePair.Value;
                            Application element5 = element1.Applications.CreateElement();
                            element5.Path = key;
                            element5.ApplicationPoolName = name;
                            VirtualDirectory element6 = element5.VirtualDirectories.CreateElement();
                            element6.Path = "/";
                            element6.PhysicalPath = str10;
                            element5.VirtualDirectories.Add(element6);
                            Configuration hostConfiguration = serverManager.GetApplicationHostConfiguration();
                            string str11 = "system.webServer/security/authentication/";
                            if (key == "/cmparch" || key == "/cconsole" || key == "/ccmobile")
                            {
                                ConfigurationSection section1 = hostConfiguration.GetSection(str11 + "anonymousAuthentication", index + key);
                                section1.OverrideMode = OverrideMode.Allow;
                                section1["enabled"] = (object)true;
                                ConfigurationSection section2 = hostConfiguration.GetSection(str11 + "basicAuthentication", index + key);
                                section2.OverrideMode = OverrideMode.Allow;
                                section2["enabled"] = (object)false;
                                ConfigurationSection section3 = hostConfiguration.GetSection(str11 + "windowsAuthentication", index + key);
                                section3.OverrideMode = OverrideMode.Allow;
                                section3["enabled"] = (object)false;
                            }
                            if (key == "/cconsolesso")
                            {
                                ConfigurationSection section1 = hostConfiguration.GetSection(str11 + "anonymousAuthentication", index + key);
                                section1.OverrideMode = OverrideMode.Allow;
                                section1["enabled"] = (object)false;
                                ConfigurationSection section2 = hostConfiguration.GetSection(str11 + "basicAuthentication", index + key);
                                section2.OverrideMode = OverrideMode.Allow;
                                section2["enabled"] = (object)false;
                                ConfigurationSection section3 = hostConfiguration.GetSection(str11 + "windowsAuthentication", index + key);
                                section3.OverrideMode = OverrideMode.Allow;
                                section3["enabled"] = (object)true;
                            }
                            if (key == "/cmparchintra")
                            {
                                ConfigurationSection section1 = hostConfiguration.GetSection(str11 + "anonymousAuthentication", index + key);
                                section1.OverrideMode = OverrideMode.Allow;
                                section1["enabled"] = (object)true;
                                ConfigurationSection section2 = hostConfiguration.GetSection(str11 + "basicAuthentication", index + key);
                                section2.OverrideMode = OverrideMode.Allow;
                                section2["enabled"] = (object)false;
                                ConfigurationSection section3 = hostConfiguration.GetSection(str11 + "windowsAuthentication", index + key);
                                section3.OverrideMode = OverrideMode.Allow;
                                section3["enabled"] = (object)true;
                            }
                            element1.Applications.Add(element5);
                        }
                        serverManager.CommitChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                Program.w(ex.Message + (ex.InnerException != null ? ex.InnerException.Message : ""));
            }
        }

        public static void w(string s)
        {
            Console.WriteLine(s);
        }
    }
}
