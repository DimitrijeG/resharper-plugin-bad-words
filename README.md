# BadWordsReSharperPlugin for Rider and ReSharper

[![Rider](https://img.shields.io/jetbrains/plugin/v/RIDER_PLUGIN_ID.svg?label=Rider&colorB=0A7BBB&style=for-the-badge&logo=rider)](https://plugins.jetbrains.com/plugin/RIDER_PLUGIN_ID)
[![ReSharper](https://img.shields.io/jetbrains/plugin/v/RESHARPER_PLUGIN_ID.svg?label=ReSharper&colorB=0A7BBB&style=for-the-badge&logo=resharper)](https://plugins.jetbrains.com/plugin/RESHARPER_PLUGIN_ID)

This plugin highlights inappropriate phrases in comments and provides quick-fix suggestions to replace them. Replacement phrases are sourced from a directory containing `.text` files with pairs formatted as `bad_phrase ==> good_replacement`.

The plugin acquires the directory path through an environment variable and tracks any changes to the files. The bad words repository updates the phrase replacement dictionary asynchronously to prevent interference with user typing.

## Getting Started

1. Clone the repository:
   ```bash
   git clone https://github.com/DimitrijeG/resharper-plugin-bad-words
   ```
2. Change to the directory:
   ```bash
   git clone https://github.com/DimitrijeG/resharper-plugin-bad-words
   ```
3. Start your IDE with the plugin, setting the environment variable `BAD_WORDS_DIR` to your chosen bad words directory path
