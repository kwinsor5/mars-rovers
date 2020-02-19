using System;

using MarsRovers2.Enums;

namespace MarsRovers2.Rovers {
	public sealed class Rover {

		#region Properties

		/// <summary>
		/// Gets the rover identifier.
		/// </summary>
		/// <value>The rover identifier.</value>
		public Guid RoverID { get; private set; }

		/// <summary>
		/// Gets the name of the rover.
		/// </summary>
		/// <value>The name of the rover.</value>
		public string RoverName { get; private set; }

		/// <summary>
		/// Gets the direction this rover is facing.
		/// </summary>
		/// <value>The direction.</value>
		public Directions Direction { get; private set; }

		/// <summary>
		/// Gets the current x-coordinate of this rover.
		/// </summary>
		/// <value>The x.</value>
		public int X { get; private set; }

		/// <summary>
		/// Gets the current y-coordinate of this rover.
		/// </summary>
		/// <value>The y.</value>
		public int Y { get; private set; }

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="Rover"/> class.
		/// </summary>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		/// <param name="direction">The direction.</param>
		public Rover(int x, int y, string roverName, Directions direction) {
			this.RoverID = Guid.NewGuid();
			this.X = x;
			this.Y = y;
			this.RoverName = roverName;
			this.Direction = direction;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Moves the rover forward.
		/// </summary>
		public void Move(int boundaryE, int boundaryN) {
			switch (this.Direction) {
				case Directions.N: {
						if (this.Y + 1 > boundaryN) throw new InvalidOperationException("The rover will fall off the north edge if you do this!");
						this.Y++;
					}
					break;
				case Directions.S: {
						if (this.Y - 1 < 0) throw new InvalidOperationException("The rover will fall off the south edge if you do this!");
						this.Y--;
					}
					break;
				case Directions.W: {
						if (this.X - 1 < 0) throw new InvalidOperationException("The rover will fall off the west edge if you do this!");
						this.X--;
					}
					break;
				case Directions.E: {
						if (this.X + 1 > boundaryE) throw new InvalidOperationException("The rover will fall off the east edge if you do this!");
						this.X++;
					}
					break;
			}
		}

		/// <summary>
		/// Spins the rover to face the specified direction.
		/// </summary>
		/// <param name="direction">The direction.</param>
		public void Spin(char direction) {
			if (direction == 'R') {
				switch (this.Direction) {
					case Directions.N: {
							this.Direction = Directions.E;
						}
						break;
					case Directions.S: {
							this.Direction = Directions.W;
						}
						break;
					case Directions.E: {
							this.Direction = Directions.S;
						}
						break;
					case Directions.W: {
							this.Direction = Directions.N;
						}
						break;
				}
			}
			else if (direction == 'L') {
				switch (this.Direction) {
					case Directions.N: {
							this.Direction = Directions.W;
						}
						break;
					case Directions.S: {
							this.Direction = Directions.E;
						}
						break;
					case Directions.E: {
							this.Direction = Directions.N;
						}
						break;
					case Directions.W: {
							this.Direction = Directions.S;
						}
						break;
				}
			}
			else {
				throw new InvalidOperationException("Please provide a valid direction.");
			}
		}

		#endregion

	}
}
