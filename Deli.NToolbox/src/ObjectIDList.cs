using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FistVR;

namespace NToolbox
{
    public class ObjectIDList
    {
        public FileInfo File { get; }
        public IEnumerable<string> List => System.IO.File.ReadAllLines(File.FullName).ToList();
        public int Length => List.ToArray().Length;

        public ObjectIDList(string filename)
        {
            List<string> objIds = IM.OD.Select(kvp => kvp.Key).ToList();

            const string NTOOLBOX_DIR = "NToolbox";

            if (!Directory.Exists(NTOOLBOX_DIR)) Directory.CreateDirectory(NTOOLBOX_DIR);

            FileInfo file = new(Path.Combine(NTOOLBOX_DIR, filename));

            using StreamWriter writer = file.CreateText();
            foreach (var obj in objIds) writer.WriteLine(obj);
            writer.Dispose();

            File = file;
        }

        public FVRObject this[int index] => IM.OD[List.ToArray()[index]];
        public FVRObject this[string name] => IM.OD[name];
    }
}