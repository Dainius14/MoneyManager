<template>
    <v-content class="bg">
        <v-container class="fill-height" fluid>
            <v-row align="center" justify="center">
                <v-col cols="12" sm="8" md="4">
                    <v-card class="elevation-12">
                        <v-toolbar color="primary" dark flat>
                            <v-toolbar-title>Login form</v-toolbar-title>
                            <v-spacer />
                        </v-toolbar>
                        <v-form v-model="isFormValid" @submit.prevent="onLogin">
                            <v-card-text>
                                    <v-text-field
                                        label="Email"
                                        prepend-icon="mdi-account"
                                        type="email"
                                        v-model="email"
                                        :rules="emailRules"
                                    />

                                    <v-text-field
                                        label="Password"
                                        prepend-icon="mdi-key"
                                        type="password"
                                        v-model="password"
                                        :rules="passwordRules"
                                    />
                            </v-card-text>
                            <v-card-actions>
                                <v-spacer />
                                <v-btn
                                    color="primary"
                                    :disabled="!isFormValid || loggingIn"
                                    :loading="loggingIn"
                                    type="submit"
                                >Login</v-btn>
                            </v-card-actions>
                        </v-form>
                    </v-card>
                </v-col>
            </v-row>
        </v-container>
    </v-content>
</template>

<script src="./login.view.ts" />

<style>
    .bg {
        background-image: linear-gradient(120deg, #48c6ef 0%, #6f86d6 100%);
    }
</style>
