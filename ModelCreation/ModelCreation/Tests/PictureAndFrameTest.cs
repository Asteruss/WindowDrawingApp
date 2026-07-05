namespace ModelCreation.Tests
{
    public class PictureAndFrameTest
    {
        [Fact]
        public void TestCreatePicture()
        {
            Picture pic = new Picture(1800, 700);
            Assert.Equal(1800, pic.Width);
            Assert.Equal(700, pic.Height);
        }

        [Fact]
        public void TestWindowFrameSizeVertical()
        {
            Picture pic = new Picture(700, 1800);

            Assert.Equal(50, pic.WindowFrame.Left.Width);
            Assert.Equal(1800, pic.WindowFrame.Left.Length);

            Assert.Equal(50, pic.WindowFrame.Top.Width);
            Assert.Equal(600, pic.WindowFrame.Top.Length);
        }

        [Fact]
        public void TestWindowFrameSizeVerticalResize()
        {
            Picture pic = new Picture(700, 1800);

            pic.WindowFrame.Right.Width = 100;

            Assert.Equal(50, pic.WindowFrame.Left.Width);
            Assert.Equal(1800, pic.WindowFrame.Left.Length);

            Assert.Equal(100, pic.WindowFrame.Right.Width);
            Assert.Equal(1800, pic.WindowFrame.Right.Length);

            Assert.Equal(50, pic.WindowFrame.Top.Width);
            Assert.Equal(550, pic.WindowFrame.Top.Length);

            Assert.Equal(50, pic.WindowFrame.Bottom.Width);
            Assert.Equal(550, pic.WindowFrame.Bottom.Length);
        }

        [Fact]
        public void TestWindowFrameSwitchToHorizontal()
        {
            Picture pic = new Picture(700, 1800);
            pic.WindowFrame.FrameType = FrameType.Horizontal;

            Assert.Equal(50, pic.WindowFrame.Left.Width);
            Assert.Equal(1700, pic.WindowFrame.Left.Length);

            Assert.Equal(50, pic.WindowFrame.Right.Width);
            Assert.Equal(1700, pic.WindowFrame.Right.Length);

            Assert.Equal(50, pic.WindowFrame.Top.Width);
            Assert.Equal(700, pic.WindowFrame.Top.Length);

            Assert.Equal(50, pic.WindowFrame.Bottom.Width);
            Assert.Equal(700, pic.WindowFrame.Bottom.Length);
        }

        [Fact]
        public void TestLightSpaseResize()
        {
            Picture pic = new Picture(700, 1800);

            Assert.Equal(600, pic.WindowFrame.RootSpace.Width);
            Assert.Equal(1700, pic.WindowFrame.RootSpace.Height);

            pic.WindowFrame.Left.Width = 100;

            Assert.Equal(550, pic.WindowFrame.RootSpace.Width);
            Assert.Equal(1700, pic.WindowFrame.RootSpace.Height);

            pic.WindowFrame.FrameType = FrameType.Horizontal;

            Assert.Equal(550, pic.WindowFrame.RootSpace.Width);
            Assert.Equal(1700, pic.WindowFrame.RootSpace.Height);
        }

    }
    // Builder pattern
    // импосты и деление пространства
    // метод resize для окна и пространст
}