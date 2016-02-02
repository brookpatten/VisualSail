using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AmphibianSoftware.VisualSail.Library.Nmea
{
    public static class NmeaParser
    {
        private static NmeaDictionary _nmea;
        static NmeaParser()
        {
            _nmea = new NmeaDictionary(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("AmphibianSoftware.VisualSail.Nmea.xml"));/*"../../nmea.xml"*/
        }
        public static Dictionary<string,Dictionary<string,string>> Parse(string nmeaLine)
        {
            if (nmeaLine != null && nmeaLine != string.Empty && nmeaLine.Replace(" ","")!=string.Empty)
            {
                try
                {
                    char[] splitters = { ',' };
                    string[] parts = nmeaLine.Split(splitters);
                    string header = parts[0];
                    string deviceCode;
                    string sentenceType;
                    if (header[0] != '$' && header[0]!='!')
                    {
                        throw new Exception("Invalid Header");
                    }
                    deviceCode = header.Substring(1, 2);
                    sentenceType = header.Substring(3);

                    string sentence = _nmea.FindByCode(sentenceType).Name;

                    if (_nmea.FindByCode(sentenceType).Ignore)
                    {
                        throw new NMEAIgnoredSentenceException();
                    }

                    Dictionary<string, Dictionary<string, string>> parsed = new Dictionary<string, Dictionary<string, string>>();
                    parsed[sentence] = new Dictionary<string, string>();
                    for (int i = 1; i < parts.Length; i++)
                    {
                        if (i < _nmea.FindByCode(sentenceType).PartCount)
                        {
                            string name = _nmea.FindByCode(sentenceType).FindPartByPosition(i).Name;
                            string value = parts[i];
                            if (i == parts.Length - 1)
                            {
                                char[] checksumSplit = { '*' };
                                value = value.Split(checksumSplit)[0];
                            }
                            parsed[sentence][name] = value;
                        }
                    }
                    return parsed;
                }
                catch (NMEAIgnoredSentenceException e)
                {
                    throw e;
                }
                catch (KeyNotFoundException)
                {
                    throw new NMEAUnkownSentenceException();
                }
                catch (Exception e)
                {
                    throw new Exception("Invalid NMEA Data, " + e.Message);
                }
            }
            else
            {
                throw new NMEAEmptySentenceException();
            }
        }
        public static NmeaSentenceType GetSentenceTypeByName(string name)
        {
            return _nmea.FindByName(name);
        }
        public static NmeaSentenceType GetSentenceTypeByCode(string code)
        {
            return _nmea.FindByCode(code);
        }
    }
}
public class NMEAUnkownSentenceException:Exception
{
    public NMEAUnkownSentenceException():base("This NMEA Sentence Type is unknown")
    {
    }
}
public class NMEAIgnoredSentenceException : Exception
{
    public NMEAIgnoredSentenceException():base("This NMEA Sentence Type is flagged to be ignored")
    {
    }
}
public class NMEAEmptySentenceException:Exception
{
    public NMEAEmptySentenceException():base("The Row was empty or contained only spaces")
    {
    }
}