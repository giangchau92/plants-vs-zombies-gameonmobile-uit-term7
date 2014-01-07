using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SCSEngine.Control;
using SCSEngine.Services;
using SCSEngine.Sprite;
using SCSEngine.Utils.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantsVsZombies.GrowSystem
{
    public delegate void ChooseSystemEventHandler(PvZChooseSystem button);

    public class PvZChooseSystem : GameComponent
    {
        private enum ChooseSystemState
        {
            OUTSIDE,
            COMMING_IN,
            INSIDE,
            COMMING_OUT
        }

        private const float ButtonWidth = 52, ButtonHeight = 56, ButtonPadding = 4;

        private PvZGrowButtonFactoryBank buttonFB;
        private PvZChooseList waitList, chosenList;
        private Button readyButton;
        private UIControlManager uiManager;
        private IPvZGameCurrency _currency;


        private ChooseSystemState state;

        private float moveDuration = 1.5f;
        private float moveTime;
        private float insidePosition = 0f, outsidePosition = -470f;
        private float readyButtonMargin = 190f;
        private float moveVelocity;

        public event ChooseSystemEventHandler OnCameOut;

        public PvZChooseSystem(Game game, PvZGrowButtonFactoryBank btnFB, UIControlManager uiMan, IPvZGameCurrency currency)
            : base(game)
        {
            this.buttonFB = btnFB;
            this.uiManager = uiMan;
            _currency = currency;
        }

        public override void Initialize()
        {
            this.waitList = new PvZChooseList(this.Game, ButtonWidth, ButtonHeight, ButtonPadding,
                this.uiManager);
            this.chosenList = new PvZChooseList(this.Game, ButtonWidth, ButtonHeight, ButtonPadding,
                this.uiManager);
            this.waitList.Canvas.Bound.Position = new Vector2(outsidePosition, 150);
            this.waitList.Canvas.Bound.Size = new Vector2(480, 200);
            this.waitList.Canvas.Content.Position = new Vector2(20, 40);
            this.waitList.Canvas.Content.Size = new Vector2(430, 150);
            this.waitList.Background = SCSServices.Instance.ResourceManager.GetResource<ISprite>("ChoosePlant");

            this.chosenList.Canvas.Bound.Position = new Vector2(outsidePosition, 400);
            this.chosenList.Canvas.Bound.Size = new Vector2(460, 70);
            this.chosenList.Canvas.Content.Position = new Vector2(90, 5);
            this.chosenList.Canvas.Content.Size = new Vector2(440, 56);
            this.chosenList.Background = SCSServices.Instance.ResourceManager.GetResource<ISprite>("BuyPlant");

            foreach (var buttonF in this.buttonFB)
            {
                var chButton = buttonF.Value.CreateChooseButton(this.Game);
                chButton.OnTap += this.OnChooseButtonInWaitListTapped;
                this.waitList.AddChooseButton(chButton);
            }
            this.waitList.ReArrange();

            this.uiManager.Add(this.waitList);
            this.uiManager.Add(this.chosenList);

            this.readyButton = new Button(this.Game, SCSServices.Instance.SpriteBatch,
                SCSServices.Instance.ResourceManager.GetResource<Texture2D>("Ready"),
                SCSServices.Instance.ResourceManager.GetResource<Texture2D>("ReadyOver"));
            readyButton.IsOverlay = true;
            readyButton.FitSizeByImage();
            readyButton.Canvas.Bound.Position = new Vector2(outsidePosition + readyButtonMargin, 360f);
            readyButton.OnTouched += this.OnReadyButtonTouched;
            this.uiManager.Add(readyButton);

            this.state = ChooseSystemState.OUTSIDE;

            base.Initialize();
        }

        private void OnReadyButtonTouched(Button button)
        {
            this.ComeOut();
        }

        public void ComeIn()
        {
            if (this.state == ChooseSystemState.OUTSIDE)
            {
                this.state = ChooseSystemState.COMMING_IN;
                float delta = insidePosition - this.waitList.Canvas.Bound.Position.X;
                this.moveVelocity = delta / this.moveDuration;
                this.moveTime = this.moveDuration;
            }
        }

        public void ComeOut()
        {
            if (this.state == ChooseSystemState.INSIDE)
            {
                this.state = ChooseSystemState.COMMING_OUT;
                float delta = outsidePosition - this.waitList.Canvas.Bound.Position.X;
                this.moveVelocity = delta / this.moveDuration;
                this.moveTime = this.moveDuration;
            }
        }

        public void OnChooseButtonInWaitListTapped(PvZChooseButton button)
        {
            RectangleF destRect = this.chosenList.MakeDestination();
            if (!destRect.IsEmpty && !button.IsDisabled)
            {
                PvZChooseButtonAvatar avatar = new PvZChooseButtonAvatar(this.Game, 1f, 0.2f, 0.5f);
                avatar.Background = button.Background;
                avatar.Canvas.Bound.Alter(button.Canvas.Bound);
                avatar.Parent = button;
                avatar.OnCompleteAnimating += this.OnAvatarCompleted;
                this.uiManager.Add(avatar);
                avatar.StartAnimating(destRect);

                button.IsDisabled = true;
            }
        }

        public void OnChooseButtonInChosenListTapped(PvZChooseButton button)
        {
            var chButton = this.waitList.GetButton(button.Name);
            if (chButton != null)
            {
                chButton.IsDisabled = false;
            }

            this.chosenList.RemoveChooseButton(button);
        }

        private void OnAvatarCompleted(PvZChooseButtonAvatar avatar)
        {
            avatar.KillMe();

            var buttonF = this.buttonFB[avatar.Parent.Name];
            var chButton = buttonF.CreateChooseButton(this.Game);
            chButton.OnTap += this.OnChooseButtonInChosenListTapped;
            this.chosenList.AddChooseButton(chButton);
        }

        public override void Update(GameTime gameTime)
        {
            switch (this.state)
            {
                case ChooseSystemState.COMMING_IN:
                    float velRate = this.moveVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    this.waitList.Canvas.Bound.Position.X += velRate;
                    this.chosenList.Canvas.Bound.Position.X += velRate;
                    this.readyButton.Canvas.Bound.Position.X += velRate;
                    this.waitList.ReArrange();
                    this.chosenList.ReArrange();
                    this.moveTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                    if (this.moveTime < 0)
                    {
                        this.state = ChooseSystemState.INSIDE;
                        this.waitList.Canvas.Bound.Position.X = insidePosition;
                        this.chosenList.Canvas.Bound.Position.X = insidePosition;
                        this.readyButton.Canvas.Bound.Position.X = insidePosition + readyButtonMargin;
                        this.waitList.ReArrange();
                        this.chosenList.ReArrange();
                    }
                    break;
                case ChooseSystemState.COMMING_OUT:
                    velRate = this.moveVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    this.waitList.Canvas.Bound.Position.X += velRate;
                    this.readyButton.Canvas.Bound.Position.X += velRate;
                    this.waitList.ReArrange();
                    this.moveTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                    if (this.moveTime < 0)
                    {
                        this.state = ChooseSystemState.OUTSIDE;
                        this.waitList.Canvas.Bound.Position.X = outsidePosition;
                        this.readyButton.Canvas.Bound.Position.X = outsidePosition + readyButtonMargin;
                        this.waitList.ReArrange();

                        if (this.OnCameOut != null)
                        {
                            this.OnCameOut(this);
                        }
                    }
                    break;
                default:
                    break;
            }
            base.Update(gameTime);
        }

        public PvZGrowList MakeGrowList()
        {
            var growList = new PvZGrowList(this.Game, this.chosenList.ElementWidth, this.chosenList.ElemPad, this.uiManager, _currency);
            growList.Canvas.Bound.Alter(this.chosenList.Canvas.Bound);
            growList.Canvas.Content.Alter(this.chosenList.Canvas.Content);
            growList.Background = this.chosenList.Background;

            foreach (var button in this.chosenList.ChooseButtons)
            {
                growList.AddGrowButton(this.buttonFB[button.Name].CreateButton(this.Game));
            }

            return growList;
        }

        public void RemoveAll()
        {
            this.waitList.RemoveAll();
            this.chosenList.RemoveAll();

            this.uiManager.Remove(this.chosenList);
            this.uiManager.Remove(this.waitList);
            this.uiManager.Remove(this.readyButton);
        }
    }
}
