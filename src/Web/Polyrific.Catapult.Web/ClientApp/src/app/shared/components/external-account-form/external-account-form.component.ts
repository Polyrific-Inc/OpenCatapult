import { Component, OnInit, Input, OnChanges, SimpleChanges } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { ExternalAccountTypeDto } from '@app/core/models/account/external-account-type-dto';
import { AccountService } from '@app/core';

@Component({
  selector: 'app-external-account-form',
  templateUrl: './external-account-form.component.html',
  styleUrls: ['./external-account-form.component.css']
})
export class ExternalAccountFormComponent implements OnInit, OnChanges {
  @Input() form: FormGroup;
  @Input() disableForm: boolean;
  @Input() externalAccountIds: { [key: string]: string };
  externalAccountTypes: ExternalAccountTypeDto[];

  externalAccountForm: FormGroup;

  constructor(private fb: FormBuilder, private accountService: AccountService) { }

  ngOnInit() {
    this.externalAccountIds = this.externalAccountIds || {};
    this.externalAccountForm = this.fb.group({});

    this.accountService.getExternalAccountTypes()
      .subscribe(data => {
        for (const externalAccountType of data) {
          this.externalAccountForm.setControl(externalAccountType.key,
            this.fb.control({value: this.externalAccountIds[externalAccountType.key], disabled: this.disableForm}));
        }

        this.form.setControl('externalAccountIds', this.externalAccountForm);
        this.externalAccountTypes = data;
      });
  }

  ngOnChanges(changes: SimpleChanges) {

    if (changes.disableForm && !changes.disableForm.firstChange) {
      if (this.disableForm) {
        this.externalAccountForm.patchValue(this.externalAccountIds);
        this.externalAccountForm.disable();
      } else {
        this.externalAccountForm.enable();
      }
    }
  }

}
