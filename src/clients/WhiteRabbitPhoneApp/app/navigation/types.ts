//стек страниц

import { NavigatorScreenParams } from "@react-navigation/native";
import { PostType } from "../components/Api/post.interface";
import { HomeStackParam } from "./Navigation";

export type RootStackParamList = {
    Auth: undefined;
    //Home: undefined;
    //Profile: undefined;
    About: undefined;
    Favorites: undefined;
    Booked: undefined;
    ApiTest: undefined;
    Create: undefined;
    Logout: undefined;

    ProfileStack: NavigatorScreenParams<ProfileStackParam>;
    HomeStack: NavigatorScreenParams<HomeStackParam>
    AnimalScreen: {
      id: number
      name: string
      avatar: string
      onLiked: boolean;
      onBooked: boolean;
      //post: PostType
    }
    AnimalCard: {
      name: string
      age: number
      avatar: string
    }

    
    
  };

  export type ProfileStackParam = {
    Profile: undefined;
    EditProfile: undefined;
  }

  export type AuthStackParam = {
    AuthScreen: undefined;
    Auth: undefined;
  }
  