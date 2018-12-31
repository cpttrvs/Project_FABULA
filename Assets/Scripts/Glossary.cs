using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glossary {

	public enum Language { FR, EN };

	public static Language language = Language.EN;

	public static string STRING_KEYBOARDLOCK { get {
		switch(language){
			case Language.FR: 
				return "Ecriture : ";
			case Language.EN:
				return "Writing: ";
			default:
				return "";
		}
	}}
	public static string STRING_LIFE { get {
		switch(language){
			case Language.FR: 
				return "Vie : ";
			case Language.EN:
				return "Life: ";
			default:
				return "";
		}
	}}

	public static string STRING_RESUME { get {
		switch(language){
			case Language.FR: 
				return "REPRENDRE";
			case Language.EN:
				return "RESUME";
			default:
				return "";
		}
	}}

    public static string STRING_RESTART
    {
        get
        {
            switch (language)
            {
                case Language.FR:
                return "RECOMMENCER";
                case Language.EN:
                return "RESTART";
                default:
                return "";
            }
        }
    }

    public static string STRING_BACK { get {
		switch(language){
			case Language.FR: 
				return "RETOUR";
			case Language.EN:
				return "BACK";
			default:
				return "";
		}
	}}

	public static string STRING_VOCABULARY { get {
		switch(language){
			case Language.FR: 
				return "VOCABULAIRE";
			case Language.EN:
				return "VOCABULARY";
			default:
				return "";
		}
	}}

	public static string STRING_QUIT { get {
		switch(language){
			case Language.FR: 
				return "QUITTER";
			case Language.EN:
				return "QUIT";
			default:
				return "";
		}
	}}

	public static string STRING_NBCORRECTWORDS { get {
		switch(language){
			case Language.FR: 
				return "Nombre de mots corrects : ";
			case Language.EN:
				return "Number of correct words: ";
			default:
				return "";
		}
	}}

	public static string STRING_NBERRORS { get {
		switch(language){
			case Language.FR: 
				return "Nombre d'erreurs : ";
			case Language.EN:
				return "Number of errors: ";
			default:
				return "";
		}
	}}

    public static string STRING_WPM
    {
        get
        {
            switch (language)
            {
                case Language.FR:
                return "Mots par minute : ";
                case Language.EN:
                return "Words per minute: ";
                default:
                return "";
            }
        }
    }

    public static string STRING_LANGUAGE { get {
		switch(language){
			case Language.FR: 
				return "LANGUE";
			case Language.EN:
				return "LANGUAGE";
			default:
				return "";
		}
	}}

    public static string STRING_WIN
    {
        get
        {
            switch (language)
            {
                case Language.FR:
                return "Vous avez gagné ! ";
                case Language.EN:
                return "You won ! ";
                default:
                return "";
            }
        }
    }

    public static string STRING_LOOSE
    {
        get
        {
            switch (language)
            {
                case Language.FR:
                return "Vous êtes mort... ";
                case Language.EN:
                return "You died... ";
                default:
                return "";
            }
        }
    }

    public static string STRING_MOVEMENT
    {
        get
        {
            switch (language)
            {
                case Language.FR:
                return "Z, Q, S, D\npour bouger";
                case Language.EN:
                return "Z, Q, S, D\nto move";
                default:
                return "";
            }
        }
    }

    public static string STRING_ENTER
    {
        get
        {
            switch (language)
            {
                case Language.FR:
                return "ENTRÉE    pour écrire";
                case Language.EN:
                return "ENTER          to write";
                default:
                return "";
            }
        }
    }
}
