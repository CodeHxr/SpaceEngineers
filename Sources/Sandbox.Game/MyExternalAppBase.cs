using System;
using System.Collections.Generic;
using VRageMath;

using Sandbox;
using Sandbox.Graphics.TransparentGeometry.Particles;
using Sandbox.Engine.Utils;
using Sandbox.Engine.Platform;

using Sandbox.Common;
using VRage;
using VRage.Utils;
using Sandbox.Definitions;
using Sandbox.Game.World;

namespace Sandbox.AppCode
{
    public class MyExternalAppBase : IExternalApp
    {
        internal static MySandboxGame Static;

        static bool m_isEditorActive;
        public static bool IsEditorActive
        {
            get { return m_isEditorActive; }
            set { m_isEditorActive = value;}
        }


        static bool m_isPresent = false;
        public static bool IsPresent
        {
            get { return m_isPresent; }
            set
            {
                m_isPresent = value;
            }
        }

        public void Run(VRageGameServices services, IntPtr windowHandle, bool customRenderLoop = false, MySandboxGame game = null )
        {
            MyLog.Default = MySandboxGame.Log;

            if (game == null)
            {
                Static = new MySandboxExternal(this, services, null, windowHandle);
            }
            else
            {
                Static = game;
            }

            Initialize(Static);

            LoadDefinitions();

            //Sandbox.Definitions.MyDefinitionManager.Static.LoadData(new List<Sandbox.Common.ObjectBuilders.MyObjectBuilder_Checkpoint.ModItem>());

            //Static.In
            Static.OnGameLoaded += GameLoaded;

            //GameLoaded(this, null);

            Static.Run(customRenderLoop);            

            if (!customRenderLoop)
            {
                Dispose();
            }
        }

        public void Dispose()
        {
            Static.Dispose();
            Static = null;
        }

        public void RunSingleFrame()
        {
            Static.RunSingleFrame();
        }

        public void EndLoop()
        {
            Static.EndLoop();
        }

        void IExternalApp.Draw()
        {
            Draw(false);
        }

        void IExternalApp.Update()
        {
            Update(true);
        }

        void IExternalApp.UpdateMainThread()
        {
            UpdateMainThread();
        }

        public virtual void Initialize(Sandbox.Engine.Platform.Game game)
        {
        }

        public virtual void UpdateMainThread()
        {
            
        }

        public virtual void Update(bool canDraw)
        {
        }

        public virtual void Draw(bool canDraw)
        {
        }

        public virtual void GameLoaded(object sender, EventArgs e)
        {
            IsEditorActive = true;
            IsPresent = true;
        }


        public MyParticleEffect CreateParticle(int id)
        {
            MyParticleEffect effect = MyParticlesLibrary.CreateParticleEffect(id);
            return effect;
}

        public void RemoveParticle(MyParticleEffect effect)
        {
            effect.Clear();
            MyParticlesLibrary.RemoveParticleEffectInstance(effect);
        }

        public MatrixD GetSpectatorMatrix()
        {
            MatrixD worldMatrix = MatrixD.Invert(MySpectatorCameraController.Static.GetViewMatrix());
            return worldMatrix;
        }

        public MyParticleGeneration AllocateGeneration()
        {
            return MyParticlesManager.GenerationsPool.Allocate();
        }

        public MyParticleEffect CreateLibraryEffect()
        {
            MyParticleEffect effect = MyParticlesManager.EffectsPool.Allocate();
            return effect;
        }

        public void AddParticleToLibrary(MyParticleEffect effect)
        {
            MyParticlesLibrary.AddParticleEffect(effect);
        }

        public void UpdateParticleLibraryID(int ID)
        {
            MyParticlesLibrary.UpdateParticleEffectID(ID);
        }

        public void RemoveParticleFromLibrary(int ID)
        {
            MyParticlesLibrary.RemoveParticleEffect(ID);
        }

        public IEnumerable<MyParticleEffect> GetLibraryEffects()
        {
            return MyParticlesLibrary.GetParticleEffects();
        }


        public void SaveParticlesLibrary(string file)
        {
            MyParticlesLibrary.Serialize(file);
        }

        public void LoadParticlesLibrary(string file)
        {
			// Before Loading Particles Library the definitions need to initialized first!
            MyDefinitionManager.Static.LoadData(new List<Sandbox.Common.ObjectBuilders.MyObjectBuilder_Checkpoint.ModItem>());
            MyParticlesLibrary.Deserialize(file);
        }

        public void LoadDefinitions()
        {
            // this is needed for render materials to be loaded
            MyDefinitionManager.Static.LoadData(new List<Sandbox.Common.ObjectBuilders.MyObjectBuilder_Checkpoint.ModItem>());            
        }



        public float GetStepInSeconds()
        {
            return MyEngineConstants.UPDATE_STEP_SIZE_IN_SECONDS;
        }
    }
}
