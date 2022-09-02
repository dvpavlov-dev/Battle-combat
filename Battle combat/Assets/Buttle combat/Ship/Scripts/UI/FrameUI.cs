namespace ShipModule
{

    using UnityEngine;
    using UnityEngine.UI;

    public class FrameUI : MonoBehaviour
    {
        public RawImage ImgFrame;
        public Texture FrameSelectPlayer;
        public Texture FrameSelectEnemy;

        void Start()
        {
            ImgFrame.enabled = false;
        }

        public void TurnOnFrame(Ship.ShipRole role)
        {
            switch (role)
            {
                case Ship.ShipRole.Player:
                    ImgFrame.enabled = true;
                    ImgFrame.texture = FrameSelectPlayer;
                    break;

                case Ship.ShipRole.Enemy:
                    ImgFrame.enabled = true;
                    ImgFrame.texture = FrameSelectEnemy;
                    break;

                default:
                    ImgFrame.enabled = false;
                    break;
            }
        }
    }

}