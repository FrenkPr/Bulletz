using System;
using System.Collections.Generic;
using Aiv.Fast2D;
using OpenTK;

namespace Bulletz
{
    static class TextMngr
    {
        private static Texture texture;
        private static Dictionary<string, string> phrases;
        private static Dictionary<string, bool> phrasesVisibility;
        private static Dictionary<string, Sprite[]> phrasesSprite;
        private static int defaultCharWidth;
        private static int defaultCharHeight;
        private static Dictionary<char, Vector2> charsSpriteOffset;
        private static int charsAsciiCode;

        public static void Init()
        {
            charsAsciiCode = 32;  //starting char = space

            texture = TextureMngr.GetTexture("textImage");
            phrases = new Dictionary<string, string>();
            phrasesSprite = new Dictionary<string, Sprite[]>();
            phrasesVisibility = new Dictionary<string, bool>();
            defaultCharWidth = texture.Width / 10;
            defaultCharHeight = texture.Height / 10;
            charsSpriteOffset = new Dictionary<char, Vector2>(95);
            float startX = 0;
            float startY = 0;

            for (int i = 0; i < 10; i++)
            {
                int numCol = i == 9 ? 5 : 10;

                for (int j = 0; j < numCol; j++)
                {
                    charsSpriteOffset.Add((char)charsAsciiCode, new Vector2(startX, startY));
                    startX += defaultCharWidth;
                    charsAsciiCode++;
                }

                startX = 0;
                startY += defaultCharHeight;
            }
        }

        public static void AddPhraseAt(string id, string phrase, Vector2 startPos, int charWidth = 0, int charHeight = 0, bool visible = true)
        {
            charWidth = charWidth <= 0 ? defaultCharWidth : charWidth;
            charHeight = charHeight <= 0 ? defaultCharHeight : charHeight;

            if (phrase == "" || phrase == null || id == "" || id == null)
            {
                return;
            }

            if (phrasesSprite.ContainsKey(id))
            {
                return;
            }

            if (startPos == null)
            {
                startPos = new Vector2(0);
            }

            Sprite[] phraseSprite = new Sprite[phrase.Length];
            Vector2 currentPos = startPos;

            for (int i = 0; i < phrase.Length; i++)
            {
                phraseSprite[i] = new Sprite(charWidth, charHeight);

                if (phrase[i] == '\n')
                {
                    currentPos.X = startPos.X;
                    currentPos.Y += charHeight;

                    phraseSprite[i].position = currentPos;
                }

                else
                {
                    phraseSprite[i].position = currentPos;

                    currentPos.X += charWidth;
                }
            }

            phrases.Add(id, phrase);
            phrasesSprite.Add(id, phraseSprite);
            phrasesVisibility.Add(id, visible);
        }

        public static bool IsPhraseVisible(string id)
        {
            if (!phrasesVisibility.ContainsKey(id))
            {
                return false;
            }

            return phrasesVisibility[id];
        }

        public static void SetPhraseVisibility(string id, bool value)
        {
            if (!phrasesVisibility.ContainsKey(id))
            {
                return;
            }

            phrasesVisibility[id] = value;
        }

        public static void RemovePhrase(string id)
        {
            if (id == null)
            {
                return;
            }

            if (phrases.ContainsKey(id))
            {
                phrases.Remove(id);
                phrasesSprite.Remove(id);
                phrasesVisibility.Remove(id);
            }
        }

        public static void EditPhrase(string oldId, string newPhrase, string newId = "")
        {
            if (oldId == null || newId == null || newPhrase == null || newPhrase == "")
            {
                return;
            }

            if (!phrases.ContainsKey(oldId))
            {
                return;
            }

            Vector2 position = phrasesSprite[oldId][0].position;
            int charWidth = (int)phrasesSprite[oldId][0].Width;
            int charHeight = (int)phrasesSprite[oldId][0].Height;
            newId = newId == "" ? oldId : newId;

            bool visibility = phrasesVisibility[newId];

            RemovePhrase(oldId);
            AddPhraseAt(newId, newPhrase, position, charWidth, charHeight, visibility);
        }

        public static void ClearAll()
        {
            phrases = null;
            phrasesSprite = null;
            phrasesVisibility = null;
            texture = null;
            charsSpriteOffset = null;
            defaultCharWidth = 0;
            defaultCharHeight = 0;
            charsAsciiCode = 0;
        }

        public static void PrintPhrases()
        {
            int i = 0;
            string[] phrasesId = new string[phrases.Count];

            foreach (string id in phrases.Keys)
            {
                phrasesId[i] = id;
                i++;
            }

            i = 0;

            foreach (Sprite[] phraseSprite in phrasesSprite.Values)
            {
                for (int j = 0; j < phraseSprite.Length; j++)
                {
                    if (!phrasesVisibility[phrasesId[i]])
                    {
                        break;
                    }

                    if (phrases[phrasesId[i]][j] == '\n')
                    {
                        continue;
                    }

                    phraseSprite[j].DrawTexture(texture, (int)charsSpriteOffset[phrases[phrasesId[i]][j]].X, (int)charsSpriteOffset[phrases[phrasesId[i]][j]].Y, defaultCharWidth, defaultCharHeight);
                }

                i++;
            }
        }
    }
}
