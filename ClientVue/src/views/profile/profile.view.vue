<template>
    <div>
        <v-card>
            <v-form v-model="isChangeEmailFormValid" @submit.prevent="onChangeEmail">
                <v-card-text>
                    <v-text-field
                        label="Email"
                        prepend-icon="mdi-account"
                        type="email"
                        v-model="email"
                        :rules="emailRules"
                    />
                </v-card-text>

                <v-card-actions>
                    <v-btn color="primary"
                           :disabled="!isChangeEmailFormValid || changingEmail"
                           :loading="changingEmail"
                           type="submit"
                    >Change email</v-btn>
                </v-card-actions>
            </v-form>
        </v-card>


        <v-card class="mt-4">
            <v-form v-model="isChangePasswordFormValid" @submit.prevent="onChangePassword" ref="form">
                <v-card-text>
                    <v-text-field
                            label="Current password"
                            prepend-icon="mdi-key"
                            type="password"
                            v-model="currentPassword"
                            :rules="passwordRules"
                    />
                    <v-text-field
                            label="New password"
                            prepend-icon="mdi-key"
                            type="password"
                            v-model="newPassword"
                            :rules="newPasswordRules"
                            @input="$refs.form.validate()"
                    />
                    <v-text-field
                            label="Repeat new password"
                            prepend-icon="mdi-key"
                            type="password"
                            v-model="repeatNewPassword"
                            :rules="newPasswordRules"
                            @input="$refs.form.validate()"
                    />
                </v-card-text>
                <v-card-actions>
                    <v-btn
                        color="primary"
                        :disabled="!isChangePasswordFormValid || changingEmail"
                        :loading="changingEmail"
                        type="submit"
                    >Update</v-btn>
                </v-card-actions>
            </v-form>
        </v-card>
    </div>
</template>

<script src="./profile.view.ts" />

