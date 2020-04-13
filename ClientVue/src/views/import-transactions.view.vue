<template>
    <div>
        <v-file-input
            v-model="file"
            accept="text/csv,text/comma-separated-values,application/csv,.csv"
            label="Csv file"
        ></v-file-input>

        <v-btn
            @click="onUploadButtonClicked"
            color="primary"
            :disabled="file.size === 0"
            :loading="isUploading"
        >
            <v-icon left>mdi-file-upload</v-icon> Upload
        </v-btn>

            <v-alert v-if="importComplete" type="success" text>
                <h3 class="headline">Successfully imported the file</h3>
                <ul>
                    <li>Created {{ importResults.createdTransactions }} transactions.</li>
                    <li>Created {{ importResults.createdPersonalAccounts }} personal accounts.</li>
                    <li>Created {{ importResults.createdOtherAccounts }} other accounts.</li>
                    <li>Created {{ importResults.createdCategories }} categories.</li>
                </ul>
            </v-alert>
    </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import { TransactionsApi } from '@/services/transactions.api';
import { ImportTransactionsResults } from '@/models/import-transactions-results.model';

@Component({})
export default class ImportTransactionsView extends Vue {
    isUploading: boolean = false;
    file: File = new File([], '');
    importComplete = false;
    importResults: ImportTransactionsResults = {} as any;

    async onUploadButtonClicked() {
        this.isUploading = true;
        this.importResults = await TransactionsApi.uploadFile(this.file);
        this.importComplete = true;
        this.isUploading = false;
        this.file = new File([], '');
    }
}
</script>
