# PdfiumNet

[![License: Apache 2.0](https://img.shields.io/badge/license-Apache%202-blue)](LICENSE)


## Overview

**PdfiumNet** is a modern, .NET wrapper around Google’s PDFium native library. It is a heavily modified fork of:

* [pvginkel/PdfiumViewer](https://github.com/pvginkel/PdfiumViewer) (Apache 2.0)
* [bezzad/PdfiumViewer WPF port](https://github.com/bezzad/PdfiumViewer) (Apache 2.0)


PdfiumNet implements comprehensive PDF loading, rendering, and interaction features for .NET applications without bundling the native PDFium DLL.
It integrates many memory fix, text search improvement and a general optimized method to load .PDF files. (Render3, for instance)


## Features

* Load and render PDF documents in WPF applications
* Navigate pages, zoom, and search text
* Customizable rendering options (anti-aliasing, DPI)
* Efficient memory management with native interop
* Support for annotations and form fields
* Thread-safe preview and printing support


## Getting Started

### Prerequisites

* .NET Core 3.1+ / .NET 6+
* WPF application environment
* Native PDFium binaries (installed via Nuget reference after first build)

### Installation

1. Clone or install PdfiumNet via Git:

   ```bash
   git clone https://github.com/Alessio2405/PdfiumNet.git
   ```
2. Open `PdfiumNet.csproj` in Visual Studio or your preferred IDE.
3. Build the project. Ensure the native `pdfium.dll` is available in project’s output directory.
4. Reference the `PdfiumNet` project or DLL in your application.

### Usage

```xml
<Window x:Class="PdfViewerSample.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:pdf="clr-namespace:PdfiumNet;assembly=PdfiumNet"
        Title="PdfiumNet Sample" Height="600" Width="800">
    <Grid>
        <pdf:PdfRenderer x:Name="Viewer"/>
    </Grid>
</Window>
```

```csharp
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        Viewer?.OpenPdf("src\sample.pdf");
    }
}
```

### Configuration

You can adjust rendering settings in code:

```csharp
Viewer.RenderingOptions = new RenderingOptions
{
    EnableAntialiasing = true,
    DpiX = 96,
    DpiY = 96
};
```

## Contributing

Contributions are welcome! Please follow these guidelines:

1. Fork the repository.
2. Create a feature branch (`git checkout -b feature/YourFeature`).
3. Commit your changes (`git commit -m "Add feature"`).
4. Push to the branch (`git push origin feature/YourFeature`).
5. Open a Pull Request describing your change.

> **Note:** All upstream code is licensed under Apache 2.0. Please preserve existing license headers and the `NOTICE` file in modifications.

## License

This project is licensed under the **Apache License, Version 2.0**. See the [LICENSE](LICENSE) file for details.

---
