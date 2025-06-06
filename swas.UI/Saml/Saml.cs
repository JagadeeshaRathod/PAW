using SAMLSignatureVerification;
using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;

/// <summary>
/// Summary description for saml
/// </summary>

namespace OneLogin
{
    namespace Saml
    {
        public class Certificate
        {
            public X509Certificate2 cert;

            public void LoadCertificate(string certificate)
            {
                cert = new X509Certificate2(StringToByteArray(certificate));
               // cert.Import(StringToByteArray(certificate));
            }

            public void LoadCertificate(byte[] certificate)
            {
                cert = new X509Certificate2();
                cert.Import(certificate);
            }

            private byte[] StringToByteArray(string st)
            {
                byte[] bytes = new byte[st.Length];
                for (int i = 0; i < st.Length; i++)
                {
                    bytes[i] = (byte)st[i];
                }
                return bytes;
            }
        }

        public class Response
        {
            private XmlDocument xmlDoc;
            private AccountSettings accountSettings;
            private Certificate certificate;

            public Response(AccountSettings accountSettings)
            {
                this.accountSettings = accountSettings;
                certificate = new Certificate();
                certificate.LoadCertificate(accountSettings.certificate);
            }

            public void LoadXml(string xml)
            {
                xmlDoc = new XmlDocument();
                xmlDoc.PreserveWhitespace = true;
                xmlDoc.XmlResolver = null;
                xmlDoc.LoadXml(xml);
            }

            public void LoadXmlFromBase64(string response)
            {
                if (response != null)// anil
                {
                    System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
                    LoadXml(enc.GetString(Convert.FromBase64String(response)));
                }

            }

            public bool IsValid_sign()
            {
                bool status = true;
                if (xmlDoc != null)
                {
                    #region signatureVerification
                    SignatureVerification ss = new SignatureVerification();
                    string result=   ss.ValidateSignature(xmlDoc.DocumentElement, "C:\\Cert\\IAM Public Key\\class3.cer", "emudhra");
                    if(result=="True")
                    {
                        XmlNamespaceManager manager = new XmlNamespaceManager(xmlDoc.NameTable);
                        manager.AddNamespace("ds", SignedXml.XmlDsigNamespaceUrl);
                        manager.AddNamespace("saml", "urn:oasis:names:tc:SAML:2.0:assertion");
                        manager.AddNamespace("samlp", "urn:oasis:names:tc:SAML:2.0:protocol");
                        XmlNodeList nodeList = xmlDoc.SelectNodes("//ds:Signature", manager);

                        var notBefore = NotBefore();
                       
                        var notOnOrAfter = NotOnOrAfter();
                        status &= !notOnOrAfter.HasValue|| (notOnOrAfter > DateTime.Now);

                        if (status == true)
                        {
                            return validateSAML1();
                        }
                        else
                        {
                          return  status;
                        }
                    }
                    else
                    {
                        return false;
                    }
                    #endregion
                }
                else
                {
                    return false;
                }
               
            }

            public bool IsValid()
            {
                bool status = true;
                if (xmlDoc != null)
                {
                    #region signatureVerification
                   
                        XmlNamespaceManager manager = new XmlNamespaceManager(xmlDoc.NameTable);
                        manager.AddNamespace("ds", SignedXml.XmlDsigNamespaceUrl);
                        manager.AddNamespace("saml", "urn:oasis:names:tc:SAML:2.0:assertion");
                        manager.AddNamespace("samlp", "urn:oasis:names:tc:SAML:2.0:protocol");
                        XmlNodeList nodeList = xmlDoc.SelectNodes("//ds:Signature", manager);

                        var notBefore = NotBefore();

                        var notOnOrAfter = NotOnOrAfter();
                        status &= !notOnOrAfter.HasValue || (notOnOrAfter > DateTime.Now);

                        if (status == true)
                        {
                            return validateSAML();
                        }
                        else
                        {
                            return status;
                        }
                   
                    #endregion
                }
                else
                {
                    return false;
                }

            }


            public DateTime? NotBefore()
            {
                XmlNamespaceManager manager = new XmlNamespaceManager(xmlDoc.NameTable);
                manager.AddNamespace("saml", "urn:oasis:names:tc:SAML:2.0:assertion");
                manager.AddNamespace("samlp", "urn:oasis:names:tc:SAML:2.0:protocol");

                var nodes = xmlDoc.SelectNodes("/samlp:Response/saml:Assertion/saml:Conditions", manager);
                string value = null;
                if (nodes != null && nodes.Count > 0 && nodes[0] != null && nodes[0].Attributes != null && nodes[0].Attributes["NotBefore"] != null)
                {
                    value = nodes[0].Attributes["NotBefore"].Value;
                }
                return value != null ? DateTime.Parse(value) : (DateTime?)null;
            }

            public DateTime? NotOnOrAfter()
            {
                XmlNamespaceManager manager = new XmlNamespaceManager(xmlDoc.NameTable);
                manager.AddNamespace("saml", "urn:oasis:names:tc:SAML:2.0:assertion");
                manager.AddNamespace("samlp", "urn:oasis:names:tc:SAML:2.0:protocol");

                var nodes = xmlDoc.SelectNodes("/samlp:Response/saml:Assertion/saml:Conditions", manager);
                string value = null;
                if (nodes != null && nodes.Count > 0 && nodes[0] != null && nodes[0].Attributes != null && nodes[0].Attributes["NotOnOrAfter"] != null)
                {
                    value = nodes[0].Attributes["NotOnOrAfter"].Value;
                }
                return value != null ? DateTime.Parse(value) : (DateTime?)null;
            }

            public string GetNameID()
            {
                XmlNamespaceManager manager = new XmlNamespaceManager(xmlDoc.NameTable);
                manager.AddNamespace("ds", SignedXml.XmlDsigNamespaceUrl);
                manager.AddNamespace("saml", "urn:oasis:names:tc:SAML:2.0:assertion");
                manager.AddNamespace("samlp", "urn:oasis:names:tc:SAML:2.0:protocol");

                XmlNode node = xmlDoc.SelectSingleNode("/samlp:Response/saml:Assertion/saml:Subject/saml:NameID", manager);
                return node.InnerText;
            }
            public string GetEntityID()
            {
                XmlNamespaceManager manager = new XmlNamespaceManager(xmlDoc.NameTable);
                manager.AddNamespace("ds", SignedXml.XmlDsigNamespaceUrl);
                manager.AddNamespace("saml", "urn:oasis:names:tc:SAML:2.0:assertion");
                manager.AddNamespace("samlp", "urn:oasis:names:tc:SAML:2.0:protocol");

                XmlNode node = xmlDoc.SelectSingleNode("/samlp:Response/saml:Assertion/saml:Subject/saml:NameID", manager);
                return node.InnerText;
            }

            public string GetSAMLRole()
            {
                string role = string.Empty;
                XmlNamespaceManager manager = new XmlNamespaceManager(xmlDoc.NameTable);
                manager.AddNamespace("ds", SignedXml.XmlDsigNamespaceUrl);
                manager.AddNamespace("saml", "urn:oasis:names:tc:SAML:2.0:assertion");
                manager.AddNamespace("samlp", "urn:oasis:names:tc:SAML:2.0:protocol");

                XmlNodeList nodes = xmlDoc.SelectNodes("/samlp:Response/saml:Assertion/saml:AttributeStatement/saml:Attribute", manager);
                if (nodes.Count > 0)
                {
                    for(int i=0;i<nodes.Count;i++)
                    {
                        if (nodes[i].Attributes[0].InnerText.Trim() == "Application_Role")
                        {
                            role = nodes[i].InnerText.Trim();
                            break;
                        }
                    }
                }
                else
                {
                    role = string.Empty;
                }
                return role;
            }

            public bool validateSAML()
            {
                return true;

                //bool rslt = false;
                
                //XmlNamespaceManager manager = new XmlNamespaceManager(xmlDoc.NameTable);
                //manager.AddNamespace("ds", SignedXml.XmlDsigNamespaceUrl);
                //manager.AddNamespace("saml", "urn:oasis:names:tc:SAML:2.0:assertion");
                //manager.AddNamespace("samlp", "urn:oasis:names:tc:SAML:2.0:protocol");

                //var nodes = xmlDoc.SelectNodes("/samlp:Response/saml:Assertion/saml:Subject/saml:SubjectConfirmation", manager);
                //string value = null;
                //if (nodes != null)
                //{
                //    value = nodes[0].ChildNodes[0].Attributes[0].Value;
                //}
                //AccountSettings accountSettings=new AccountSettings();
                //if (accountSettings.entityId.ToUpper() == value.ToUpper())
                //{
                //    rslt = true;
                //}
                //else
                //{
                //    rslt = false;
                //}

                //return rslt;
            }

            public bool validateSAML1()
            {
                bool rslt = false;

                XmlNamespaceManager manager = new XmlNamespaceManager(xmlDoc.NameTable);
                manager.AddNamespace("ds", SignedXml.XmlDsigNamespaceUrl);
                manager.AddNamespace("saml", "urn:oasis:names:tc:SAML:2.0:assertion");
                manager.AddNamespace("samlp", "urn:oasis:names:tc:SAML:2.0:protocol");

                var nodes = xmlDoc.SelectNodes("/samlp:Response/saml:Assertion/saml:Subject/saml:SubjectConfirmation", manager);
                string value = null;
                if (nodes != null)
                {
                    value = nodes[0].ChildNodes[0].Attributes[0].Value;
                }
                AccountSettings accountSettings = new AccountSettings();
                if (accountSettings.entityId.ToUpper() == value.ToUpper())
                {
                    rslt = true;
                }
                else
                {
                    rslt = false;
                }
                return rslt;
            }

            public void GetLogoutParameter(out string Nameid, out string issuer)
            {
                XmlNamespaceManager manager = new XmlNamespaceManager(xmlDoc.NameTable);
                manager.AddNamespace("ds", SignedXml.XmlDsigNamespaceUrl);
                manager.AddNamespace("saml", "urn:oasis:names:tc:SAML:2.0:assertion");
                manager.AddNamespace("samlp", "urn:oasis:names:tc:SAML:2.0:protocol");

                XmlNode Issuer = xmlDoc.SelectSingleNode("/samlp:LogoutRequest/saml:Issuer", manager);
                XmlNode NameID = xmlDoc.SelectSingleNode("/samlp:LogoutRequest/saml:NameID", manager);

                Nameid = NameID.InnerText.Trim();
                issuer = Issuer.InnerText.Trim();
            }
        }

        public class AuthRequest
        {
            public string id;
            public string url;
            private string issue_instant;
            private AppSettings appSettings;
            private AccountSettings accountSettings;

            public enum AuthRequestFormat
            {
                Base64 = 1
            }
            public AuthRequest(AppSettings appSettings, AccountSettings accountSettings)
            {
                this.appSettings = appSettings;
                this.accountSettings = accountSettings;
                id = accountSettings.entityId;
                url = accountSettings.idp_sso_target_url;
                issue_instant = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
            }

            public string GetRequest(AuthRequestFormat format)
            {
                using (StringWriter sw = new StringWriter())
                {
                    XmlWriterSettings xws = new XmlWriterSettings();
                    xws.OmitXmlDeclaration = true;

                    using (XmlWriter xw = XmlWriter.Create(sw, xws))
                    {
                        xw.WriteStartElement("samlp", "AuthnRequest", "urn:oasis:names:tc:SAML:2.0:protocol");
                        xw.WriteAttributeString("entityId", id);
                        xw.WriteAttributeString("singleSignOnService", url);

                        xw.WriteAttributeString("Version", "2.0");
                        xw.WriteAttributeString("IssueInstant", issue_instant);
                        xw.WriteAttributeString("ProtocolBinding", "urn:oasis:names:tc:SAML:2.0:bindings:HTTP-POST");
                        xw.WriteAttributeString("assertionConsumerService", appSettings.assertionConsumerServiceUrl);

                        xw.WriteStartElement("saml", "Issuer", "urn:oasis:names:tc:SAML:2.0:assertion");
                        xw.WriteString(appSettings.issuer);
                        xw.WriteEndElement();

                        xw.WriteStartElement("samlp", "NameIDPolicy", "urn:oasis:names:tc:SAML:2.0:protocol");
                        xw.WriteAttributeString("NameIDFormat", "urn:oasis:names:tc:SAML:2.0:nameid-format:unspecified");
                        xw.WriteAttributeString("AllowCreate", "true");
                        xw.WriteEndElement();

                        xw.WriteStartElement("samlp", "RequestedAuthnContext", "urn:oasis:names:tc:SAML:2.0:protocol");
                        xw.WriteAttributeString("Comparison", "exact");

                        xw.WriteStartElement("saml", "AuthnContextClassRef", "urn:oasis:names:tc:SAML:2.0:assertion");
                        xw.WriteString("urn:oasis:names:tc:SAML:2.0:ac:classes:PasswordProtectedTransport");
                        xw.WriteEndElement();

                        xw.WriteEndElement(); // RequestedAuthnContext

                        xw.WriteEndElement();
                    }

                    if (format == AuthRequestFormat.Base64)
                    {
                        byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(sw.ToString());
                        return System.Convert.ToBase64String(toEncodeAsBytes);
                    }
                    return null;
                }
            }

            public string GetLogOutRequest(AuthRequestFormat format, string issuer, string logouturl)
            {
                using (StringWriter sw = new StringWriter())
                {
                    XmlWriterSettings xws = new XmlWriterSettings();
                    xws.OmitXmlDeclaration = true;

                    using (XmlWriter xw = XmlWriter.Create(sw, xws))
                    {
                        xw.WriteStartElement("saml2p", "LogoutResponse", "urn:oasis:names:tc:SAML:2.0:protocol");
                        xw.WriteAttributeString("ID", id);

                        xw.WriteAttributeString("Version", "2.0");
                        xw.WriteAttributeString("IssueInstant", issue_instant);
                        xw.WriteAttributeString("Destination", issuer);
                        xw.WriteAttributeString("InResponseTo", logouturl);
                        //xw.WriteAttributeString("ProtocolBinding", "urn:oasis:names:tc:SAML:2.0:bindings:HTTP-POST");
                        //xw.WriteAttributeString("assertionConsumerService", appSettings.assertionConsumerServiceUrl);

                        xw.WriteStartElement("saml2", "Issuer", "urn:oasis:names:tc:SAML:2.0:assertion");
                        xw.WriteString(appSettings.issuer);
                        xw.WriteEndElement();

                        xw.WriteStartElement("saml2p", "Status", "urn:oasis:names:tc:SAML:2.0:protocol");
                        xw.WriteStartElement("saml2p", "StatusCode", "urn:oasis:names:tc:SAML:2.0:status:Success");
                        xw.WriteAttributeString("Value", "Success");
                        xw.WriteEndElement();
                        xw.WriteEndElement();

                        //xw.WriteStartElement("samlp", "RequestedAuthnContext", "urn:oasis:names:tc:SAML:2.0:protocol");
                        //xw.WriteAttributeString("Comparison", "exact");

                        //xw.WriteStartElement("saml", "AuthnContextClassRef", "urn:oasis:names:tc:SAML:2.0:assertion");
                        //xw.WriteString("urn:oasis:names:tc:SAML:2.0:ac:classes:PasswordProtectedTransport");
                        //xw.WriteEndElement();

                        xw.WriteEndElement(); // RequestedAuthnContext

                        // xw.WriteEndElement();
                    }

                    if (format == AuthRequestFormat.Base64)
                    {
                        byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(sw.ToString());
                        return System.Convert.ToBase64String(toEncodeAsBytes);
                    }

                    return null;
                }
            }

            public string SingleLogoutRequest1(AuthRequestFormat format, string issuer, string AppRole,string username)
            {
                using (StringWriter sw = new StringWriter())
                {
                    XmlWriterSettings xws = new XmlWriterSettings();
                    xws.OmitXmlDeclaration = true;

                    using (XmlWriter xw = XmlWriter.Create(sw, xws))
                    {
                        xw.WriteStartElement("saml2p", "LogoutRequest", "urn:oasis:names:tc:SAML:2.0:protocol");
                        xw.WriteAttributeString("ID", id);

                        xw.WriteAttributeString("Version", "2.0");
                        xw.WriteAttributeString("NameID", username);
                      //  xw.WriteAttributeString("Issuer", issuer);
                       // xw.WriteAttributeString("AppRole", logouturl);
                        //xw.WriteAttributeString("ProtocolBinding", "urn:oasis:names:tc:SAML:2.0:bindings:HTTP-POST");
                        //xw.WriteAttributeString("assertionConsumerService", appSettings.assertionConsumerServiceUrl);

                        xw.WriteStartElement("saml2", "Issuer", "urn:oasis:names:tc:SAML:2.0:assertion");
                        xw.WriteString(appSettings.issuer);
                        xw.WriteEndElement();
                        xw.WriteStartElement("saml2", "AppRole", "urn:oasis:names:tc:SAML:2.0:assertion");
                        xw.WriteString(AppRole);
                        xw.WriteEndElement();

                        //xw.WriteStartElement("saml2p", "Status", "urn:oasis:names:tc:SAML:2.0:protocol");
                        //xw.WriteStartElement("saml2p", "StatusCode", "urn:oasis:names:tc:SAML:2.0:status:Success");
                        //xw.WriteAttributeString("Value", "Success");
                        //xw.WriteEndElement();
                        //xw.WriteEndElement();

                        //xw.WriteStartElement("samlp", "RequestedAuthnContext", "urn:oasis:names:tc:SAML:2.0:protocol");
                        //xw.WriteAttributeString("Comparison", "exact");

                        //xw.WriteStartElement("saml", "AuthnContextClassRef", "urn:oasis:names:tc:SAML:2.0:assertion");
                        //xw.WriteString("urn:oasis:names:tc:SAML:2.0:ac:classes:PasswordProtectedTransport");
                        //xw.WriteEndElement();

                        xw.WriteEndElement(); // RequestedAuthnContext

                        // xw.WriteEndElement();
                    }

                    if (format == AuthRequestFormat.Base64)
                    {
                        byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(sw.ToString());
                        return System.Convert.ToBase64String(toEncodeAsBytes);
                    }

                    return null;
                }
            }

            public string SingleLogoutRequest(AuthRequestFormat format, string issuer, string AppRole, string username)
            {
                using (StringWriter sw = new StringWriter())
                {
                    XmlWriterSettings xws = new XmlWriterSettings();
                    xws.OmitXmlDeclaration = true;

                    using (XmlWriter xw = XmlWriter.Create(sw, xws))
                    {
                        xw.WriteStartElement("saml2p", "LogoutRequest", "urn:oasis:names:tc:SAML:2.0:protocol");
                        xw.WriteAttributeString("ID", id);

                        // xw.WriteAttributeString("Version", "2.0");
                        // xw.WriteAttributeString("IssueInstant", issue_instant);
                        xw.WriteStartElement("saml2", "AppRole", "urn:oasis:names:tc:SAML:2.0:assertion");
                        xw.WriteString(AppRole);
                        xw.WriteEndElement();
                       
                        xw.WriteStartElement("saml2", "Issuer", "urn:oasis:names:tc:SAML:2.0:assertion");
                        xw.WriteString(appSettings.issuer);
                        xw.WriteEndElement();
                        xw.WriteStartElement("saml2", "NameID", "urn:oasis:names:tc:SAML:2.0:assertion");
                        xw.WriteString(username);
                        xw.WriteEndElement();
                        xw.WriteEndElement();
                    }

                    if (format == AuthRequestFormat.Base64)
                    {
                        byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(sw.ToString());
                        return System.Convert.ToBase64String(toEncodeAsBytes);
                    }

                    return null;
                }
            }
        }
    }
}
