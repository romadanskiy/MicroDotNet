package com.example.scanner

import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import androidx.lifecycle.ViewModelProviders
import com.example.scanner.databinding.FragmentScannerBinding
import com.example.scanner.models.ScanningResultViewModel
import com.google.android.material.bottomnavigation.BottomNavigationView

class MainActivity : AppCompatActivity() {

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)


        var currentFragmentId = -1;
        var bottomNav  = findViewById<BottomNavigationView>(R.id.bottom_navigation);

        bottomNav.setOnNavigationItemSelectedListener { item ->
            when (item.itemId) {
                R.id.nav_scanner -> {
                    if(currentFragmentId != R.id.nav_scanner){
                        supportFragmentManager.beginTransaction().replace(R.id.fragment_container, ScannerFragment()).commit();
                        currentFragmentId = R.id.nav_scanner;
                    }
                }
                R.id.nav_utilization -> {
                    if(currentFragmentId != R.id.nav_utilization){
                        supportFragmentManager.beginTransaction().replace(R.id.fragment_container, UtilizationFragment()).commit();
                        currentFragmentId = R.id.nav_utilization;
                    }
                }
                R.id.nav_profile -> {
                    if(currentFragmentId != R.id.nav_profile){
                        supportFragmentManager.beginTransaction().replace(R.id.fragment_container, ProfileFragment()).commit();
                        currentFragmentId = R.id.nav_profile;
                    }
                }
                R.id.nav_login -> {
                    if(currentFragmentId != R.id.nav_login){
                        supportFragmentManager.beginTransaction().replace(R.id.fragment_container, LoginFragment()).commit();
                        currentFragmentId = R.id.nav_login;
                    }
                }
            }
            true;
        }

        supportFragmentManager.beginTransaction().replace(R.id.fragment_container, ScannerFragment()).commit();

    }
}