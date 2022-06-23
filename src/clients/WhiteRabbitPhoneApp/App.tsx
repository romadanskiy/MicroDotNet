import 'react-native-gesture-handler';
import { StatusBar } from 'expo-status-bar';
import React, { useState } from 'react';
import { LogBox } from 'react-native';
import Navigation from './app/navigation/Navigation';
import { AuthProvider } from './app/providers/AuthProvider';
import 'react-native-gesture-handler';
//import { AuthProviderOIDC } from './app/components/screens/auth/AuthProviderOIDC';
//import  {AuthProvider}  from './app/components/screens/auth/contexts/auth';



export default function App() {
  //return <More/>
  return (
      <AuthProvider>
        <Navigation/>
      </AuthProvider>
  );
}

LogBox.ignoreAllLogs()
