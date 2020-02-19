using System;
using System.Linq;

using MarsRovers2.Enums;
using MarsRovers2.Rovers;

namespace MarsRovers2 {

	/// <summary>
	/// Class Program.
	/// </summary>
	/// <remarks>
	/// Hi, DealerOn! I chose the Mars Rover question for one reason and one reason only - it sounded more exciting to work on. 
	/// Taxes bore me. Trains are cool and all but I don't have a Sheldon-esque fascination with them.
	/// 
	/// I actually completed this assessment for DealerOn in 2016 during a previous job search (the above blurb made me laugh, I was probably on a Big Bang binge at the time), 
	/// and by the time you guys had reached out for an interview I had already accepted an offer at my old company. 
	/// I was familiar with them and it happened fast.
	/// Anyway, I dug through some old repositories and found it, so I figured I'd convert it to .NET Core 3.1, refactor some code
	/// with some newer C# features, and clean it up a little bit.
	/// </remarks>
	class Program {

		const string InvalidDirectionsMessage = @"Please provide valid directions. 'L' = Left, 'R' = Right, 'M' = Move Forward.";

		static char[] StringDelimeters = new char[] { ' ' };
		static char[] AcceptedDirectionInputs = new char[] { 'L', 'R' };

		static void Main(string[] args) {
			Plateau.Plateau plateau;
			while (true) {
				try {
					plateau = ReadPlateauInput();
					break;
				}
				catch (Exception caught) {
					Console.WriteLine(caught.Message);
				}
			}
			while (plateau.Rovers.Count < 2) {
				Rover rover = null;
				while (true) {
					try {
						if (rover == null) {
							rover = InitializeRover(plateau);
						}
						MoveRover(rover, plateau.BoundaryE, plateau.BoundaryN);
						rover = null; // Success! set rover to null to initialize the next instance
						break;
					}
					catch (Exception caught) {
						Console.WriteLine(caught.Message);

					}
				}
			}
			Console.WriteLine("Sequence ended. Here is where your rovers ended up:");
			foreach (var rover in plateau.Rovers) {
				Console.WriteLine($"{rover.RoverName}: {rover.X} {rover.Y} {rover.Direction}");
			}
			while (true) {
				Console.WriteLine("Press any key to exit rover navigation sequence.");
				string input = Console.ReadLine();
				if (input != null) break;
			}
		}

		/// <summary>
		/// Parses and validates the plateau grid's coordinates.
		/// </summary>
		/// <returns>Plateau.Plateau.</returns>
		static Plateau.Plateau ReadPlateauInput() {
			Console.WriteLine("What are the coordinates for the upper-right hand corner of the plateau?");
			string input = Console.ReadLine();

			#region ValidateUserInput

			if (string.IsNullOrWhiteSpace(input))
				throw new InvalidOperationException("Please provide valid coordinates.");

			string[] splitCoordinates = input.Split(StringDelimeters, StringSplitOptions.RemoveEmptyEntries);

			if (!int.TryParse(splitCoordinates[0], out int boundaryE)) {
				throw new InvalidCastException("Please provide valid coordinates.");
			}
			if (boundaryE < 0) {
				throw new ArgumentOutOfRangeException("The plateau's boundary must be a positive integer.");
			}

			if (!int.TryParse(splitCoordinates[1], out int boundaryN)) {
				throw new InvalidCastException("Please provide valid coordinates.");
			}
			if (boundaryN < 0) {
				throw new ArgumentOutOfRangeException("The plateau's boundary must be a positive integer.");
			}

			#endregion

			return new Plateau.Plateau(Guid.NewGuid(), boundaryE, boundaryN);
		}

		/// <summary>
		/// Parses and validates the rover's inital location.
		/// </summary>
		/// <param name="plateau">The plateau.</param>
		static Rover InitializeRover(Plateau.Plateau plateau) {
			Console.WriteLine($"Where is the {(plateau.Rovers.Count > 0 ? "next " : string.Empty)}rover located?");
			string input = Console.ReadLine();

			#region ValidateUserInput

			if (string.IsNullOrWhiteSpace(input)) {
				throw new InvalidCastException("Please provide valid integer coordinates.");
			}

			string[] splitCoordinates = input.Split(StringDelimeters, StringSplitOptions.RemoveEmptyEntries);

			if (!int.TryParse(splitCoordinates[0].Trim(), out int x)) {
				throw new InvalidCastException("Please provide valid integer coordinates.");
			}
			if (x < 0 || x > plateau.BoundaryE) {
				throw new ArgumentOutOfRangeException($"The x-coordinate must be a positive integer and less than or equal to the plateau's east boundary.");
			}

			if (!int.TryParse(splitCoordinates[1], out int y)) {
				throw new InvalidCastException("Please provide valid integer coordinates.");
			}
			if (y < 0 || y > plateau.BoundaryN) {
				throw new ArgumentOutOfRangeException($"The x-coordinate must be a positive integer and less than or equal to the plateau's north boundary.");
			}

			string directionString = splitCoordinates[2]?.ToUpperInvariant();
			if (!Enum.TryParse(directionString ?? string.Empty, out Directions direction)) {
				throw new InvalidOperationException("Please provide a valid direction.");
			}

			#endregion

			Guid roverID = plateau.Rovers.Add(x, y, direction);
			var rover = plateau.Rovers[roverID];
			Console.WriteLine($"Rover '{rover.RoverName}' landed. Where should it go?");

			return rover;

		}

		/// <summary>
		/// Parses and validates the direction input and moves the rover accordingly.
		/// </summary>
		/// <param name="rover">The rover.</param>
		/// <param name="directions">The directions.</param>
		static void MoveRover(Rover rover, int boundaryE, int boundaryN) {
			string directions = Console.ReadLine();
			

			#region ValidateDirectionInput

			if (string.IsNullOrWhiteSpace(directions)) {
				throw new InvalidOperationException(InvalidDirectionsMessage);
			}

			foreach (char item in directions.ToUpperInvariant()) {
				if (!AcceptedDirectionInputs.Contains(item) && item != 'M') {
					throw new InvalidOperationException(InvalidDirectionsMessage);
				}
			}

			#endregion

			try {
				foreach (char item in directions.ToUpperInvariant()) {
					if (item == 'M') {
						rover.Move(boundaryE, boundaryN);
					}
					else {
						rover.Spin(item);
					}
				}
			}
			catch (Exception caught) {
				Console.WriteLine(caught.Message);
				Console.WriteLine($"{rover.RoverName} made it to {rover.X},{rover.Y} before it could not follow directions any longer.");
				Console.WriteLine($"Finish moving {rover.RoverName} (but be more careful this time!).");
				MoveRover(rover, boundaryE, boundaryN);
			}
		}
	}
}
