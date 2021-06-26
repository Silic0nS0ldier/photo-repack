# Photo Repack

A simple tool which processes images into a more compact form, with (hopefully) zero data loss. Unsupported files will be copied.

To use;

1. Clone (no binary is currently available)
2. `dotnet run -- --input-dir ./input-path --output-dir ./output-path

## Roadmap

### Xplat Paths

Rework paths to have a high level of cross environment compatibility on public interfaces and universal path formats internally (using `file://` approach).

### Video Support

Effectively encoding a video file for size is a taxing process, particularly for a mobile device which increasingly are the source of such content. Hardware level video processing improvements help, but will always run into a wall somewhere (plus this does nothing for existing content).

Support for repacking video files might provide significant disk space savings in some circumstances, provided the right formats are supported and algoirthms in place.

This is not an easy feature to implement right, and will be taxing on hardware. Early bail-out functionality is a must.

### Alternate Compression Libraries

There are a lot of compression libraries in the wild, particularly beyond the .NET ecosystem. Using these provides an opportunity for greater space savings.

WASM is likely the most portable approach, however FFI generation is an issue. Langauges like C and Rust tend to generate code for the interop (usuallyy JS with optional TS annotations) which makes consumption from different languages more difficult (though not impossible provided WASM can be used).

* Squoosh
  Has C source which can compile into WASM.
* Photon
  Rust library with WASM target.

### File Format Changing

Some file formats (especially newer ones) can compress better, on example is JXL which is intended to supersede JPEG.

### Compress Known Digital Negatives

Digital negatives are most valuable for post processing, and are often unused. Support for compressing these using a general purpose archive may provide significant savings. It should be possible to repack if reprocessed (to enable existing photo collections to further enhanced as improvements are made to the tool).
