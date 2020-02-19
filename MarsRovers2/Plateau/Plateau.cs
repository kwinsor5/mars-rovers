using System;

using MarsRovers2.Rovers;

namespace MarsRovers2.Plateau {

	/// <summary>
	/// Class Plateau.
	/// </summary>
	public sealed class Plateau {

		#region Properties

		public Guid PlateauID { get; private set; }
		public int BoundaryE { get; private set; }
		public int BoundaryN { get; private set; }
		public RoverCollection Rovers { get; private set; }

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="Plateau"/> class.
		/// </summary>
		/// <param name="boundaryN">The boundary n.</param>
		/// <param name="boundaryE">The boundary e.</param>
		public Plateau(Guid plateauID, int boundaryE, int boundaryN) {
			this.PlateauID = plateauID;
			this.BoundaryE = boundaryE;
			this.BoundaryN = boundaryN;
			this.Rovers = new RoverCollection(plateauID);
		}

		#endregion

	}
}
