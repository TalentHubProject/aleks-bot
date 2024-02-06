plugins {
    application
}

repositories {
    mavenCentral()
    maven("https://jitpack.io/")
}

dependencies {
    implementation("com.google.guava:guava:32.1.1-jre")

    implementation("net.dv8tion:JDA:5.0.0-alpha.1")

    implementation("ch.qos.logback:logback-classic:1.4.12")
}

testing {
    suites {
        val test by getting(JvmTestSuite::class) {
            useJUnit("4.13.2")
        }
    }
}

java {
    toolchain {
        languageVersion.set(JavaLanguageVersion.of(21))
    }
}

application {
    mainClass.set("org.talenthub.App")
}
