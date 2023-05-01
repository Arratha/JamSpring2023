using UnityEngine;


namespace Drop.Liquids.Models
{
    public abstract class BaseLiquid
    {
        public Color LiquidColor { get; protected set; }

        public float FactorOfFriction { get; protected set; }
        public float FactrorOfJumping { get; protected set; }
    }

    public class Water : BaseLiquid
    {
        public Water()
        {
            LiquidColor = ColorUtility.TryParseHtmlString("#241C9861", out Color tempColor) ? tempColor : Color.white;

            FactorOfFriction = 1.05f;
        }
    }

    public class Oil : BaseLiquid
    {
        public Oil()
        {
            LiquidColor = ColorUtility.TryParseHtmlString("#ECEF6B61", out Color tempColor) ? tempColor : Color.white;

            FactorOfFriction = 1;
        }
    }

    public class Pitch : BaseLiquid
    {
        public Pitch()
        {
            LiquidColor = ColorUtility.TryParseHtmlString("#55352961", out Color tempColor) ? tempColor : Color.white;

            FactorOfFriction = 1.6f;
        }
    }
}