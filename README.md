# Offside Detector

Offside Detector is a Windows Forms application that detects offside positions in soccer images using image processing techniques. The application allows users to import an image, process it to detect players and the ball, and determine offside positions.

## Table of Contents

- Features
- Installation
- Usage
- Project Structure
- Contributing
- License

## Features

- Import soccer images for processing.
- Detect players and the ball in the image.
- Determine offside positions based on player and ball positions.
- Draw offisde position line.
- Draw arrows to every player to whom the ball holder can pass.
- Visualize the results with annotated images.

## Installation

1. Clone the repository:

    ```sh
    git clone https://github.com/yourusername/offside-detector.git
    ```

2. Open the solution file `offside-detector.sln` in Visual Studio.

3. Restore the NuGet packages:

    ```sh
    dotnet restore
    ```

4. Build the project:

    ```sh
    dotnet build
    ```

## Usage

1. Run the application by pressing `F5` in Visual Studio or using the following command:

    ```sh
    dotnet run --project offside-detector
    ```

2. Click the "Import Image" button to select an image file.

3. Click the "Process Image" button to detect players and the ball, and determine offside positions.

4. The input image and the processed output image will be displayed in the application.

## Project Structure

```plaintext
offside-detector/
├── .gitattributes
├── .gitignore
├── App.config
├── Display.cs
├── Display.Designer.cs
├── Display.resx
├── Models/
│   ├── Player.cs
│   └── Team.cs
├── Program.cs
├── Properties/
│   ├── AssemblyInfo.cs
│   ├── Resources.Designer.cs
│   ├── Resources.resx
│   ├── Settings.Designer.cs
│   └── Settings.settings
├── Services/
│   ├── ImageProcessor.cs
│   └── OffsideDetector.cs
├── offside-detector.csproj
└── offside-detector.sln
```

- **Display.cs**: Contains the main form logic for the application.
- **Player.cs**: Defines the Player class and PlayerStatusenum.
- **Team.cs**: Defines the Teamclass.
- **Program.cs**: Entry point for the application.
- **ImageProcessor.cs**: Contains the logic for processing images and detecting players and the ball.
- **OffsideDetector.cs**: Contains the logic for detecting offside positions.

## Contributing

Contributions are welcome! Please follow these steps to contribute:

1. Fork the repository.
2. Create a new branch (`git checkout -b feature-branch`).
3. Make your changes.
4. Commit your changes (`git commit -m 'Add some feature'`).
5. Push to the branch (`git push origin feature-branch`).
6. Open a pull request.

## License

This project is licensed under the MIT License. See the [LICENSE](./LICENSE) file for details.
