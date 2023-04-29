using MSCLoader;
using System;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace YOUR_CS_PROJECT_NAME_HERE
{
    public class MyMod2 : Mod
    {
        public override string ID => "MyMod2"; // Your (unique) mod ID 
        public override string Name => "TEST"; // Your mod name
        public override string Author => "nevicoolomg"; // Name of the Author (your name)
        public override string Version => "1.0"; // Version
        public override string Description => "test123123"; // Short description of your mod

        public GameObject cube;
        public Keybind key;

        public override void ModSetup()
        {
            SetupFunction(Setup.OnLoad, Mod_OnLoad);
            SetupFunction(Setup.OnSave, Mod_OnSave);
            SetupFunction(Setup.Update, Mod_Update);
        }


        public override void ModSettings()
        {
            key = Keybind.Add(this, "teleport", "Teleport to cube", KeyCode.F3);
        }
        private void Mod_OnLoad()
        {
            cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.name = "cube(clone)";
            cube.transform.position = Vector3.one * 4; // 0,4,0
            cube.AddComponent<Rigidbody>();
            cube.GetComponent<Renderer>().material.color = Color.blue;
            cube.MakePickable(); // Make objects pickable so the user can actually pick up items

            if (File.Exists(Path.Combine(Application.persistentDataPath, "testmod1.xml"))) // Mod save data Check if it exists
            {
                using (FileStream stream = File.OpenRead(Path.Combine(Application.persistentDataPath, "testmod1.xml"))) //If it exists it opens/reads
                {
                    XmlSerializer save = new XmlSerializer(typeof(Save));
                    Save s = (Save)save.Deserialize(stream);
                    save.Serialize(stream, s);
                    cube.transform.position = new Vector3(s.x, s.y, s.z);
                    cube.transform.rotation = Quaternion.Euler(s.xrot, s.yrot, s.zrot);
                }
            }
        }
        private void Mod_OnSave()
        {
            using (FileStream stream = File.OpenWrite(Path.Combine(Application.persistentDataPath, "testmod1.xml"))) // This overwrites/ writes mod save
            {
                XmlSerializer save = new XmlSerializer(typeof(Save));
                Save s = new Save(cube.transform.position, cube.transform.rotation);
                save.Serialize(stream, s);
            }
        }
        private void Mod_Update()
        {
            if (key.GetKeybindDown())
            {
                GameObject player = GameObject.Find("PLAYER"); //  "PLAYER" is the deafult name
                player.transform.position = cube.transform.position + Vector3.up * 2; // Means that it goes UP from the  cube by 2
            }
        }
    }
    public class Save // Just saves rotation and Positions to the XML
    {
        public float x;
        public float y;
        public float z;

        public float xrot,
        yrot,
         zrot,
         wRot;

        public Save() { }

        public Save(Vector3 pos, Quaternion rot)
        {
            x = pos.x;
            y = pos.y;
            z = pos.z;
            xrot = rot.eulerAngles.x;
            yrot = rot.eulerAngles.y;
            zrot = rot.eulerAngles.z;

        }
    }
}
