using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NSLR_ObservationControl.Util
{
    class AnimateImage
    {
        private Image _image;
        private PictureBox _pictureBox;
        private bool _isAnimating;

        public AnimateImage(PictureBox pictureBox, string imagePath)
        {
            _pictureBox = pictureBox;
            _image = Image.FromFile(imagePath);
            _pictureBox.Image = _image;
            _isAnimating = false;
        }

        public void Play()
        {
            if (!_isAnimating)
            {
                ImageAnimator.Animate(_image, new EventHandler(OnFrameChanged));
                _isAnimating = true;
            }
        }

        public void Stop()
        {
            if (_isAnimating)
            {
                ImageAnimator.StopAnimate(_image, new EventHandler(OnFrameChanged));
                _isAnimating = false;
            }
        }

        public void Dispose()
        {
            Stop();
            _image.Dispose();
            _image = null;
            _pictureBox = null;
        }

        private void OnFrameChanged(object sender, EventArgs e)
        {
            if (_pictureBox.InvokeRequired)
            {
                _pictureBox.BeginInvoke(new Action(() => _pictureBox.Invalidate()));
            }
            else
            {
                _pictureBox.Invalidate();
            }
        }

        public bool IsAnimating
        {
            get { return _isAnimating; }
        }
    }
}

