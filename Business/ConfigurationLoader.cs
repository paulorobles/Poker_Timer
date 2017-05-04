using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Business
{
    public static class ConfigurationLoader
    {
        #region Fields and Constants.
        private const string _xmlFile = @"XmlTimerConfig.xml";
        private const string _blindsTagName = "Blinds";
        private const string _roundTagName = "round";
        private const string _numberAttribute = "number";
        private const string _anteAttribute = "ante";
        private const string _bigBlindAttribute = "bigBlind";
        private const string _smallBlindAttribute = "smallBlind";
        private const string _xPathRoundTime = "/root/roundTime";
        private const string _xPathBlindsRound = "root/Blinds/round";

        private static Configuration configuration;
        #endregion

        private static void ReadConfiguration()
        {
            configuration = new Configuration();
            // Read the XML File.

            XmlDocument _doc = new XmlDocument();
            _doc.Load(_xmlFile);

            foreach (XmlElement node in _doc.SelectNodes(_xPathBlindsRound))
            {
                // Fill blind obj.
                var blindObj = new Blind
                {
                    RoundNumber = node.GetAttribute(_numberAttribute),
                    BigBlind = node.GetAttribute(_bigBlindAttribute),
                    SmallBlind = node.GetAttribute(_smallBlindAttribute),
                    Ante = node.GetAttribute(_anteAttribute)
                };

                configuration.Blinds.Add(blindObj);
            }
            configuration.RoundTime = TimeSpan.Parse(_doc.SelectSingleNode(_xPathRoundTime).InnerText);
        }

        public static Configuration GetConfiguration()
        {
            if (configuration == null)
            {
                ReadConfiguration();
            }
            return configuration;
        }
    }
}
