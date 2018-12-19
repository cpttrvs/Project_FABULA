using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

public class Utility {

	static private string filePath = "Assets/enemies.json";
	static private List<Vocabulary> vocabularies;

	public static void InitializeVocabularyList(){
		StreamReader reader = new StreamReader(filePath);
		string json = reader.ReadToEnd();
		vocabularies = JsonConvert.DeserializeObject<List<Vocabulary>>(json);
	}

	public static List<Word> GetVocabulary(Enemy.Type enemyType, Glossary.Language language){
		foreach(Vocabulary vocabulary in vocabularies){
			if(vocabulary.type.Equals(enemyType)){
				if(language.Equals(Glossary.Language.FR)){
					return vocabulary.frWords;
				}
				else if(language.Equals(Glossary.Language.EN)){
					return vocabulary.enWords;
				}
			}
		}
		// Throw exception?
		return null;
	}
	
	private static int GetWordIndex(Glossary.Language language, Enemy.Type enemyType, string name){
		foreach(Vocabulary vocab in vocabularies){
			if(vocab.type.Equals(enemyType)){
				if(language.Equals(Glossary.Language.FR)){
					foreach(Word word in vocab.frWords){
						if(word.name.Equals(name)){
							return vocab.frWords.IndexOf(word);
						}
					}
				}
				else if(language.Equals(Glossary.Language.EN)){					
					foreach(Word word in vocab.enWords){
						if(word.name.Equals(name)){
							return vocab.enWords.IndexOf(word);
						}
					}
				}
			}
		}
		return -1;
	}
	
	public static string GetWordTranslation(Enemy.Type enemyType, Word word){
		if(Glossary.language.Equals(Glossary.Language.FR)){
			return GetVocabulary(enemyType, Glossary.Language.EN).ElementAt(GetWordIndex(Glossary.Language.FR, enemyType, word.name)).name;
		}
		else if(Glossary.language.Equals(Glossary.Language.EN)){
			return GetVocabulary(enemyType, Glossary.Language.FR).ElementAt(GetWordIndex(Glossary.Language.EN, enemyType, word.name)).name;
		}
		return "";
	}

	// Colors in red the part of the word that matches the user's input
	public static string GetWordColoring(string target, string input){
		string formatedText;
		// Part of the enemy's word beginning matches the player's input
		if(target.Length >= input.Length && target.Substring(0, input.Length).Equals(input)){
			//Debug.Log(currentWord.Substring(0, input.Length)+" = "+input);
			// We seperate the matching part from the rest
			string matchingPart = target.Substring(0, input.Length);
			string rest = target.Substring(input.Length);
			// Color the matching part
			string coloredWord = "<color=red>"+matchingPart+"</color>"+rest;
			// Update the enemy's UI text
			formatedText = coloredWord.ToUpper();
		}
		// If the input isn't EXACTLY the beginning of the word we don't hint it
		else{
			formatedText = target.ToUpper();
		}
		return formatedText;
	}

	public static string ToUpperOnFirstLetter(string text){
		return char.ToUpper(text[0]) + text.Substring(1);
	}

	public class Vocabulary
	{
		// Could be an enum
		[JsonProperty("type")]
		public Enemy.Type type;
		[JsonProperty("fr")]
		public List<Word> frWords;
		[JsonProperty("en")]
		public List<Word> enWords;
	}

	public class Word
	{
		[JsonProperty("name")]
		public string name;
		[JsonProperty("definition")]
		public string definition;
		// Added later on
		public string translation;

		public override string ToString()
		{
			return "Word: "+name+" Definition: "+definition+" Translation: "+translation;
		}
	}
}
