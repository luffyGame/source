using System;

namespace Game
{
    public class BasicModel : ObjBase
    {
        private string res;
        private AssetType assetType;
        public override ObjType objType
        {
            get { return ObjType.O_BASIC; }
        }
        public BasicModel(string res,AssetType assetType)
        {
            this.res = res;
            this.assetType = assetType;
        }
        public override void Load(Action done = null)
        {
            if (this.IsLoaded())
                return;
            rootModel = AssetModelFactory.CreateModel(assetType,res);
            rootModel.Load(done);
        }
        #region Private Method
        #endregion
    }
}