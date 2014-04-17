using System.Collections.Generic;
using MindTouch.Xml;

namespace MusicXml
{
	public class Measure
	{
		private readonly XDoc theDocument;
		private MeasureAttributes theAttributes;

		internal Measure(XDoc aDocument)
		{
			theDocument = aDocument;
		}

		public int Width
		{
			get { return theDocument["@width"].AsInt ?? -1; }
		}

		public List<Note> Notes
		{
			get
			{
				var notes = new List<Note>();
				foreach (XDoc note in theDocument["note"])
				{
					notes.Add(new Note(note));
				}
				return notes;
			}
		}

		public MeasureAttributes Attributes
		{
			get
			{
				if (theAttributes == null)
				{
					var attributes = theDocument["attributes"];
					if (!attributes.IsEmpty)
					{
						theAttributes = new MeasureAttributes(attributes);
					}
				}
				return theAttributes;
			}
		}
	}
}