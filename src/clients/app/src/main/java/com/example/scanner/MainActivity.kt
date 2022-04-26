package com.example.scanner

import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import com.google.android.material.bottomnavigation.BottomNavigationView

class MainActivity : AppCompatActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)

        var bottomNav  = findViewById<BottomNavigationView>(R.id.bottom_navigation);

        bottomNav.setOnNavigationItemSelectedListener { item ->
            when (item.itemId) {
                R.id.nav_scanner -> {
                    supportFragmentManager.beginTransaction().replace(R.id.fragment_container, ScannerFragment()).commit();
                }
                R.id.nav_utilization -> {
                    supportFragmentManager.beginTransaction().replace(R.id.fragment_container, UtilizationFragment()).commit();
                }
                R.id.nav_profile -> {
                    supportFragmentManager.beginTransaction().replace(R.id.fragment_container, ProfileFragment()).commit();
                }
                R.id.nav_login -> {
                    supportFragmentManager.beginTransaction().replace(R.id.fragment_container, LoginFragment()).commit();
                }
            }
            true;
        }

        supportFragmentManager.beginTransaction().replace(R.id.fragment_container, ScannerFragment()).commit();

    }
}