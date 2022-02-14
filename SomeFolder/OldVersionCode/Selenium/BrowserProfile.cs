/*using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Mouse_Hunter
{
    public class BrowserProfile
    {
        public async Task<string> StartProfile(string profileId)
        {
            *//*Send GET request to start the browser profile by profileId. Returns response in the following format:
            '{"status":"OK","value":"http://127.0.0.1:XXXXX"}', where XXXXX is the localhost port on which browser profile is
            launched. Please make sure that you have Multilogin listening port set to 35000. Otherwise please change the port
            value in the url string*//*
            string url = "http://127.0.0.1:35000/api/v1/profile/start?automation=true&profileId=" + profileId; //127.0.0.1 id localhost
            Uri uri = new Uri(url);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
            //--
            string responseString;
            usi ng (Stream stream = response.GetResponseStream())
            {
                us ing (StreamReader reader = new StreamReader(stream))
                //--
                {
                    responseString = reader.ReadToEnd();
                }
            }
            response.Close();
            //JObject jsonResponse = JObject.Parse(responseString);
            var fdate = JObject.Parse(responseString)["value"];
            return (string)fdate;

            ////JAVA Code///
            *//*
            String url = "http://127.0.0.1:35000/api/v1/profile/start?automation=true&profileId=" + profileId;

            URL obj = new URL(url);
            HttpURLConnection con = (HttpURLConnection)obj.openConnection();

            con.setRequestMethod("GET");
            //--
            BufferedReader in = new BufferedReader(
            new InputStreamReader(con.getInputStream()));
            //--
            String inputLine;
            StringBuffer response = new StringBuffer();

            while ((inputLine = in.readLine()) != null) {
                response.append(inputLine);
            }
            in.close();

            //Get JSON text from the response and return the value by key "value"
            JSONObject jsonResponse = new JSONObject(response.toString());
            return jsonResponse.getString("value");*//*
        }
    }
}
*/