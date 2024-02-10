using UnityEngine;

namespace MyGame.Core
{
    public class Structure : MonoBehaviour
    {
        [SerializeField]
        protected string structureName;
        public string StructureName { get { return structureName; } }

        [SerializeField] protected Sprite structurePic;
        public Sprite StructurePic { get { return structurePic; } }

        [SerializeField]
        protected int curHP;
        public int CurHP { get { return curHP; } set { curHP = value; } }

        [SerializeField]
        protected int maxHP;
        public int MaxHP { get { return maxHP; } set { maxHP = value; } }

        [SerializeField]
        protected Faction faction;

        [SerializeField]
        protected Selectable selectionVisual;
        public Selectable SelectionVisual { get { return selectionVisual; } }
    }
}