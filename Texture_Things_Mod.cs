using System;
using spaar.ModLoader;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

namespace Texture_Things_Mod
{
    public class TextureingMod : Mod
    {
        public override string Name { get { return "Texture_Things_Mod"; } }
        public override string DisplayName { get { return "Texture Things Mod"; } }
        public override string BesiegeVersion { get { return "v0.27"; } }
        public override string Author { get { return "覅是"; } }
        public override Version Version { get { return new Version("0.5"); } }
        public override bool CanBeUnloaded { get { return true; } }
        private GameObject temp = new GameObject();
        public override void OnLoad()
        {
            temp.name = "Texturing Mod";
            temp.AddComponent<Painter>();
            GameObject.DontDestroyOnLoad(temp);
        }

        public override void OnUnload()
        {
            GameObject.Destroy(temp.GetComponent<Painter>());
            GameObject.Destroy(temp);
        }


    }

    public class Painter : MonoBehaviour
    {
        public Texture TextureTempelate;
        public bool UseTransparent = false;
        public Key TextureActivationKey;
        void Start()
        {
            Commands.RegisterCommand("TextureFileName", (args, notUses) =>
            {
                try
                {
                    WWW tempwww = new WWW("File:///" + Application.dataPath + "/Mods/Blocks/Textures/" + args[0]);
                    TextureTempelate = tempwww.texture;
                    return "Done! Now texturing things will use " + args[0] + "!";
                }
                catch { return "Cannot Find the file! Please put it under " +  Application.dataPath + "/Mods/Blocks/Textures/"+" !"; }

            }, "Reset texture for texturing.");//Tf
            Commands.RegisterCommand("UseTransparting", (args, notUses) =>
            {
                UseTransparent = !UseTransparent;
                if(UseTransparent)
                {
                    return "Done! Now the textures can be transparent!";
                }
                else
                {
                    return "Done! Now the textures will not be transparent!";
                }

            }, "Reset if texturing is transparent.");//Tr
            TextureActivationKey = Keybindings.AddKeybinding("Texturing", new Key(KeyCode.LeftControl, KeyCode.T));

        }
        void Update()
        {
            if (TextureActivationKey.IsDown() ^ Input.GetKeyDown(KeyCode.F10))
            {
                TextureTempelate.wrapMode = TextureWrapMode.Clamp;
                RaycastHit hitt;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitt, float.PositiveInfinity))
                {
                        try
                        {
                                try
                                {
                            
                                hitt.transform.gameObject.GetComponent<Renderer>().material.mainTexture = null;
                            //hitt.transform.gameObject.GetComponent<Renderer>().material.color = new Color(0,0,0,0);
                            hitt.transform.gameObject.GetComponent<Renderer>().material.SetTexture("_MainTex", TextureTempelate);
                            if (UseTransparent)
                            {
                                hitt.transform.gameObject.GetComponent<Renderer>().material.shader = Shader.Find("Transparent/Diffuse");
                            }
                        }
                                catch
                                {
                            try
                            {
                                    foreach (Renderer re in hitt.transform.GetComponentsInChildren<Renderer>())
                                {
                                    re.material.mainTexture = null;
                                    //re.material.color = new Color(0,0,0,0);
                                    re.material.SetTexture("_MainTex", TextureTempelate);
                                    if (UseTransparent)
                                    { re.material.shader = Shader.Find("Transparent/Diffuse"); }
                                }
                                
                            }
                            catch { }
                        }
                            } 
                        catch (Exception e) { Debug.Log(e); }
                }
            }
        }
    }
}
