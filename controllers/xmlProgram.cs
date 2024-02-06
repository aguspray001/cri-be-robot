using CRI_Client;
using System.IO;
public class XmlProgram
{
    HardwareProtocolClient itf = new HardwareProtocolClient();
    //*****************************************************************************
    private void loadProgram(object sender, EventArgs e)
    {
        if (!itf.GetConnectionStatus())
        {
            Console.WriteLine("Send Command: Cannot send while not connected!");
            return;
        }

        string cmdText = "CMD LoadProgram ";
        cmdText += "test_matrix.xml";
        itf.SendCommand(cmdText);

    }


    //*****************************************************************************
    private void sendProgram(object sender, EventArgs e)
    {
        //if (!itf.GetConnectionStatus())
        //{
        //    log.Error("Send Program: Cannot send while not connected!");
        //    return;
        //}

        string progName = "test_matrix.xml";
        StreamReader sr;
        string line;
        string msg;

        // anzahl der Zeilen im Programm herausbekommen
        int nrOfLines = 0;
        sr = new StreamReader(progName);
        while (!sr.EndOfStream)
        {
            line = sr.ReadLine();
            nrOfLines++;
        }

        // dann übertragen
        msg = "CMD UploadFileInit ";
        msg += "Programs/" + progName;
        msg += " ";
        msg += nrOfLines;
        itf.SendCommand(msg);

        sr = new StreamReader(progName);
        for (int i = 0; i < nrOfLines; i++)
        {
            System.Threading.Thread.Sleep(10);
            line = sr.ReadLine();

            msg = "CMD UploadFileLine ";
            msg += line;
            itf.SendCommand(msg);

        }
        System.Threading.Thread.Sleep(10);
        msg = "CMD UploadFileFinish";
        itf.SendCommand(msg);
    }
}