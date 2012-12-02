using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using System.Windows.Controls;
using System.ComponentModel;
using ScriptCoreLib.Shared.Lambda;

namespace AvalonUgh.Code
{
    [Script]
    public class AnimationDictionary_Entry
    {
        public int Frame;
        public Image[] Images;
        public int Milliseconds;

        public Action Stop;
    }


	[Script]
	public class AnimationDictionary : IEnumerable<AnimationDictionary_Entry>
	{

		public void Add(int Frame, int FrameOffset)
		{
			this.Add(
				new AnimationDictionary_Entry
				{
					Frame = Frame,
					Images = new[] { this.FrameToImage(FrameOffset) }
				}
			);
		}

		public void Add(int Frame, int Milliseconds, int FrameOffset, int FrameLength)
		{
			this.Add(
				new AnimationDictionary_Entry
				{
					Frame = Frame,
					Milliseconds = Milliseconds,
					Images = Enumerable.Range(FrameOffset, FrameLength).ToArray(this.FrameToImage)
				}
			);
		}

		public void Add(AnimationDictionary_Entry e)
		{
			this.Items.Add(e);
		}

		public readonly BindingList<AnimationDictionary_Entry> Items = new BindingList<AnimationDictionary_Entry>();

		public readonly ISupportsContainer Container;
		public readonly Func<int, Image> FrameToImage;

		public AnimationDictionary(ISupportsContainer Container, Func<int, Image> FrameToImage)
		{
			this.Container = Container;
			this.FrameToImage = FrameToImage;
		}

		#region IEnumerable<Entry> Members

		public IEnumerator<AnimationDictionary_Entry> GetEnumerator()
		{
			return this.Items.GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.Items.GetEnumerator();
		}

		#endregion


		int InternalFrame;
		public int Frame
		{
			get
			{
				return InternalFrame;
			}
		}


		public void PlayFrame(int Frame)
		{
			if (InternalFrame == Frame)
				return;

			PlayFrame(Frame, null);
		}

		public void PlayFrame(int Frame, Action Done)
		{
			// we need to stop and hide any old frames

			var o = this[InternalFrame];

			if (o != null)
				if (o.Stop != null)
					o.Stop();

			InternalFrame = Frame;


			var n = this[Frame];

			if (n.Images.Length > 1)
			{
				var j = 0;
				var i = n.Images[j].AttachTo(this.Container);

				if (n.Milliseconds > 0)
				{
					var t = n.Milliseconds.AtInterval(
						delegate
						{
							j++;
							i.Orphanize();


							if (j == n.Images.Length)
							{
								j = 0;
								i = n.Images[j].AttachTo(this.Container);

								if (Done != null)
									Done();
							}
							else
							{
								i = n.Images[j].AttachTo(this.Container);
							}
						}
					);

					n.Stop =
						delegate
						{
							t.Stop();
							i.Orphanize();
						};
				}
			}
			else
			{
				var i = n.Images.Single();

				i.AttachTo(this.Container);

				n.Stop = () => i.Orphanize();
			}
		}

		public AnimationDictionary_Entry this[int Frame]
		{
			get
			{
				return this.Items.FirstOrDefault(k => k.Frame == Frame);
			}
		}
	}
}
