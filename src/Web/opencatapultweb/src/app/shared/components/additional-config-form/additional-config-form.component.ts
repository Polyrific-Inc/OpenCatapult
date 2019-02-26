import { Component, OnInit, OnChanges, Input, Output, EventEmitter, SimpleChanges } from '@angular/core';
import { TaskProviderDto, AdditionalConfigDto } from '@app/core';
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-additional-config-form',
  templateUrl: './additional-config-form.component.html',
  styleUrls: ['./additional-config-form.component.css']
})
export class AdditionalConfigFormComponent implements OnInit, OnChanges {
  @Input() taskProvider: TaskProviderDto;
  @Input() additionalConfigs: { [key: string]: string };
  @Input() disableForm: boolean;
  @Output() formReady = new EventEmitter<FormGroup>();
  additionalConfigForm = this.fb.group({});
  showForm: boolean;
  requiredAdditionalConfigs: AdditionalConfigDto[] = [];
  optionalAdditionalConfigs: AdditionalConfigDto[] = [];

  constructor(private fb: FormBuilder) {
    this.additionalConfigs = this.additionalConfigs || {};
  }

  ngOnInit() {
    this.formReady.emit(this.additionalConfigForm);
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes.taskProvider && !changes.taskProvider.firstChange) {

      if (this.taskProvider.additionalConfigs && this.taskProvider.additionalConfigs.length > 0) {
        this.showForm = true;
        for (const additionalConfig of this.taskProvider.additionalConfigs) {
          const additionalConfigValue = this.additionalConfigs ? this.additionalConfigs[additionalConfig.name] : null;
          const additionalConfigControl = additionalConfig.isRequired ? new FormControl(additionalConfigValue, Validators.required)
            : new FormControl(additionalConfigValue);
          this.additionalConfigForm.setControl(additionalConfig.name, additionalConfigControl);
        }

        this.requiredAdditionalConfigs = this.taskProvider.additionalConfigs.filter(c => c.isRequired);
        this.optionalAdditionalConfigs = this.taskProvider.additionalConfigs.filter(c => !c.isRequired);

        if (this.disableForm) {
          this.additionalConfigForm.disable();
        } else {
          this.additionalConfigForm.enable();
        }
      } else {
        this.showForm = false;
      }
    }
  }

}
